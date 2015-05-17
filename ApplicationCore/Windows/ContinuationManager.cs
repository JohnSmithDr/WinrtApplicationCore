#if WINDOWS_PHONE_APP

using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows.ApplicationModel
{
    public class ContinuationManager
    {
        IContinuationActivatedEventArgs args = null;
        bool handled = false;
        Guid id = Guid.Empty;

        internal void Continue(IContinuationActivatedEventArgs args)
        {
            Continue(args, Window.Current.Content as Frame);
        }

        internal void Continue(IContinuationActivatedEventArgs args, Frame rootFrame)
        {
            if (args == null)
                throw new ArgumentNullException("argument");

            if (this.args != null && !handled)
                throw new InvalidOperationException("Can't set argument more than once");

            this.args = args;
            this.handled = false;
            this.id = Guid.NewGuid();

            if (rootFrame == null)
                return;

            switch (args.Kind)
            {
                case ActivationKind.PickFileContinuation:
                    var fileOpenPickerPage = rootFrame.Content as IFileOpenPickerContinuable;
                    if (fileOpenPickerPage != null)
                    {
                        fileOpenPickerPage.ContinueFileOpenPicker(args as FileOpenPickerContinuationEventArgs);
                    }
                    break;

                case ActivationKind.PickSaveFileContinuation:
                    var fileSavePickerPage = rootFrame.Content as IFileSavePickerContinuable;
                    if (fileSavePickerPage != null)
                    {
                        fileSavePickerPage.ContinueFileSavePicker(args as FileSavePickerContinuationEventArgs);
                    }
                    break;

                case ActivationKind.PickFolderContinuation:
                    var folderPickerPage = rootFrame.Content as IFolderPickerContinuable;
                    if (folderPickerPage != null)
                    {
                        folderPickerPage.ContinueFolderPicker(args as FolderPickerContinuationEventArgs);
                    }
                    break;

                case ActivationKind.WebAuthenticationBrokerContinuation:
                    var wabPage = rootFrame.Content as IWebAuthenticationContinuable;
                    if (wabPage != null)
                    {
                        wabPage.ContinueWebAuthentication(args as WebAuthenticationBrokerContinuationEventArgs);
                    }
                    break;
            }
        }

        internal void MarkAsStale()
        {
            this.handled = true;
        }

        public IContinuationActivatedEventArgs ContinuationArgs
        {
            get
            {
                if (handled)
                    return null;
                MarkAsStale();
                return args;
            }
        }

        public Guid Id { get { return id; } }

        public IContinuationActivatedEventArgs GetContinuationArgs(bool includeStaleArgs)
        {
            if (!includeStaleArgs && handled)
                return null;
            MarkAsStale();
            return args;
        }
    }

    interface IFileOpenPickerContinuable
    {
        void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args);
    }

    interface IFileSavePickerContinuable
    {
        void ContinueFileSavePicker(FileSavePickerContinuationEventArgs args);
    }

    interface IFolderPickerContinuable
    {
        void ContinueFolderPicker(FolderPickerContinuationEventArgs args);
    }

    interface IWebAuthenticationContinuable
    {
        void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args);
    }
}
#endif