using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace JohnSmithDr.ApplicationCore
{
    public interface IFolderPickerContinuableOverride
    {
        void ContinueFolderPicker(StorageFolder folder, IDictionary<string, object> data);
    }
}
