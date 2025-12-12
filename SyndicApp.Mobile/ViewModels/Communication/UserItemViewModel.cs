using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SyndicApp.Mobile.ViewModels.Communication
{
    public partial class UserItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid userId;

        [ObservableProperty]
        private string nomComplet;

        // Initiales pour avatar
        public string Initiales
        {
            get
            {
                if (string.IsNullOrWhiteSpace(nomComplet))
                    return "?";

                var parts = nomComplet.Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                    return parts[0][0].ToString().ToUpper();

                return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
            }
        }

        public UserItemViewModel(Guid id, string nom)
        {
            UserId = id;
            NomComplet = nom;
        }
    }
}
