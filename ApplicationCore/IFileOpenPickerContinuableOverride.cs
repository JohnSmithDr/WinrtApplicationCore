using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace JohnSmithDr.ApplicationCore
{
    public interface IFileOpenPickerContinuableOverride
    {
        void ContinueFileOpenPicker(IReadOnlyList<StorageFile> files, IDictionary<string, object> data);
    }
}
