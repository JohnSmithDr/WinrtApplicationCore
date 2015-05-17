using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    public static class StreamExtentions
    {
        /// <summary>
        /// Copy the stream to another stream.
        /// </summary>
        public static async Task CopyToAsync(
            this Stream source,
            Stream destination,
            int bufferSize,
            CancellationToken cancellationToken,
            IProgress<int> progress)
        {
            var b = new byte[bufferSize];
            int read;
            try
            {
                while ((read = await source.ReadAsync(b, 0, bufferSize)) > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await destination.WriteAsync(b, 0, read);
                    await destination.FlushAsync();
                    progress.TryReport(read);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                b = null;
            }
        }
    }
}
