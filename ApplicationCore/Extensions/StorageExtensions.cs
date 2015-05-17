using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Windows.Storage
{
    public static class StorageExtensions
    {
        const ulong SizeThreshold = 10 * 1024 * 1024; // 10M
        const int BufferSize = 2 * 1024 * 1024; // 2M

        public static bool EqualsTo(this IStorageItem source, IStorageItem item)
        {
            if (source != null && item != null)
            {
                return string.Compare(source.Path, item.Path, StringComparison.OrdinalIgnoreCase) == 0;
            }
            return false;
        }

        public static async Task<DateTime> GetDateModifiedAsync(this IStorageItem item)
        {
            return (await item.GetBasicPropertiesAsync()).DateModified.UtcDateTime;
        }

        public static async Task<ulong> GetSizeAsync(this IStorageFile file)
        {
            return (await file.GetBasicPropertiesAsync()).Size;
        }

        public static async Task<IStorageFile> CopyAsync(
            this IStorageFile file,
            IStorageFolder folder,
            string desiredName,
            NameCollisionOption option,
            CancellationToken cancellationToken,
            IProgress<ulong> progress)
        {
            var o = (CreationCollisionOption)((int)option);
            var size = await GetSizeAsync(file);

            if (size > SizeThreshold)
            {
                var f = await folder.CreateFileAsync(desiredName, o);
                using (var fi = await file.OpenStreamForReadAsync())
                using (var fo = await f.OpenStreamForWriteAsync())
                {
                    await fi.CopyToAsync(
                        fo,
                        BufferSize,
                        cancellationToken,
                        new Progress<int>(g =>
                        {
                            progress.TryReport((ulong)g);
                        }));
                }
                return f;
            }
            else
            {
                var f = await file.CopyAsync(folder, file.Name, option);
                progress.TryReport(size);
                return f;
            }
        }

        public static async Task<ulong> SumSizeAsync(
            this IEnumerable<IStorageFile> files, 
            CancellationToken cancel)
        {
            ulong sum = 0;

            await files.ForEachAsync(async i =>
            {
                cancel.ThrowIfCancellationRequested();
                sum += (await i.GetBasicPropertiesAsync()).Size;
            });

            return sum;
        }

        public static bool IsParent(this IStorageFolder source, IStorageItem item)
        {
            var src = source.Path;
            var path = Path.GetDirectoryName(item.Path);
            while (src.Length <= path.Length && path.StartsWith(src))
            {
                if (src == path) return true;
                path = Path.GetDirectoryName(path);
            }
            return false;
        }
    }
}
