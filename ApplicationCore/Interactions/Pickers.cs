using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace JohnSmithDr.ApplicationCore.Interactions
{
    public static class Pickers
    {
#if WINDOWS_APP

        public static async Task<IStorageFile> PickFileAsync(
            PickerLocationId location,
            IEnumerable<string> filter,
            string commitButtonText = null)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = location;
            picker.FileTypeFilter.Add(filter);

            if (string.IsNullOrWhiteSpace(commitButtonText) == false)
            {
                picker.CommitButtonText = commitButtonText;
            }

            return await picker.PickSingleFileAsync();
        }

        public static async Task<IReadOnlyList<IStorageFile>> PickFilesAsync(
            PickerLocationId location,
            IEnumerable<string> filter,
            string commitButtonText = null)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = location;
            picker.FileTypeFilter.Add(filter);

            if (string.IsNullOrWhiteSpace(commitButtonText) == false)
            {
                picker.CommitButtonText = commitButtonText;
            }

            return await picker.PickMultipleFilesAsync();
        }

        public static async Task<IStorageFolder> PickFolderAsync(
            PickerLocationId location,
            string commitButtonText = null)
        {
            FolderPicker picker = new FolderPicker();
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.FileTypeFilter.Add(".pickfoldersonly");

            if (string.IsNullOrWhiteSpace(commitButtonText) == false)
            {
                picker.CommitButtonText = commitButtonText;
            }

            return await picker.PickSingleFolderAsync();
        }
#endif

#if WINDOWS_PHONE_APP

        public static void PickFileAndContinue(
            PickerLocationId location,
            IEnumerable<string> fileTypeFilter,
            IDictionary<string, object> continuationData,
            string commitButtonText = null)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = location;

            continuationData.ForEach((k, v) =>
            {
                picker.ContinuationData[k] = v;
            });

            picker.FileTypeFilter.Add(fileTypeFilter);

            if (string.IsNullOrWhiteSpace(commitButtonText) == false)
            {
                picker.CommitButtonText = commitButtonText;
            }

            picker.PickSingleFileAndContinue();
        }

        public static void PickFilesAndContinue(
            PickerLocationId location, 
            IEnumerable<string> fileTypeFilter, 
            IDictionary<string, object> continuationData, 
            string commitButtonText = null)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = location;

            continuationData.ForEach((k, v) =>
            {
                picker.ContinuationData[k] = v;
            });

            picker.FileTypeFilter.Add(fileTypeFilter);

            if (string.IsNullOrWhiteSpace(commitButtonText) == false)
            {
                picker.CommitButtonText = commitButtonText;
            }

            picker.PickMultipleFilesAndContinue();
        }

        public static void PickFolderAndContinue(
            PickerLocationId location,
            IDictionary<string, object> continuationData,
            string commitButtonText = null)
        {
            FolderPicker picker = new FolderPicker();
            picker.SuggestedStartLocation = location;

            continuationData.ForEach((k, v) =>
            {
                picker.ContinuationData[k] = v;
            });

            picker.FileTypeFilter.Add(".pickfoldersonly");

            if (string.IsNullOrWhiteSpace(commitButtonText) == false)
            {
                picker.CommitButtonText = commitButtonText;
            }

            picker.PickFolderAndContinue();
        }
#endif
    }
}
