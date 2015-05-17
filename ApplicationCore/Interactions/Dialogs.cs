using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace JohnSmithDr.ApplicationCore.Interactions
{
    public static class Dialogs
    {
        public static async Task ShowDialogAsync(
            string title, 
            string content)
        {
            var dialog = new MessageDialog(content, title);
            await dialog.ShowAsync();
        }

        public static Task ShowDialogAsync(
            ILocalizedStrings localizedStrings, 
            string titleKey, 
            string contentKey)
        {
            return ShowDialogAsync(
                localizedStrings.GetString(titleKey), 
                localizedStrings.GetString(contentKey));
        }

        public static Task ShowDialogAsync(
            ILocalizedStrings localizedStrings, 
            string titleKey, 
            string contentKey,
            params object[] contentArgs)
        {
            return ShowDialogAsync(
                localizedStrings.GetString(titleKey),
                localizedStrings.GetFormatString(contentKey, contentArgs));
        }

        public static async Task<bool> ShowConfirmAsync(
            string title, 
            string content, 
            string acceptButtonText, 
            string cancelButtonText)
        {
            var dialog = new MessageDialog(content, title);
            var acceptButton = new UICommand(acceptButtonText);
            var cancelButton = new UICommand(cancelButtonText);
            dialog.Commands.Add(acceptButton, cancelButton);
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            var result = await dialog.ShowAsync();
            return result == acceptButton;
        }

        public static Task<bool> ShowConfirmAsync(
            ILocalizedStrings localizedStrings, 
            string titleKey,
            string contentKey,
            string acceptButtonTextKey, 
            string cancelButtonTextKey)
        {
            return ShowConfirmAsync(
                localizedStrings.GetString(titleKey),
                localizedStrings.GetString(contentKey),
                localizedStrings.GetString(acceptButtonTextKey),
                localizedStrings.GetString(cancelButtonTextKey));
        }

        public static Task<bool> ShowConfirmAsync(
            ILocalizedStrings localizedStrings,
            string titleKey,
            string contentKey,
            string acceptButtonTextKey,
            string cancelButtonTextKey,
            params object[] contentArgs)
        {
            return ShowConfirmAsync(
                localizedStrings.GetString(titleKey),
                localizedStrings.GetFormatString(contentKey, contentArgs),
                localizedStrings.GetString(acceptButtonTextKey),
                localizedStrings.GetString(cancelButtonTextKey));
        }
    }
}
