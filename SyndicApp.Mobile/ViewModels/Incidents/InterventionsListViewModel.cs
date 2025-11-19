// SyndicApp.Mobile/ViewModels/Incidents/InterventionsListViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    public partial class InterventionsListViewModel : ObservableObject
    {
        private readonly IInterventionsApi _api;

        public InterventionsListViewModel(IInterventionsApi api)
        {
            _api = api;
            Items = new();
            _allItems = new();
            Statuts = Enum.GetValues(typeof(StatutIntervention))
                          .Cast<StatutIntervention>()
                          .ToList();
        }

        private List<InterventionDto> _allItems;

        [ObservableProperty] private List<InterventionDto> items;
        [ObservableProperty] private string? searchText;
        [ObservableProperty] private StatutIntervention? selectedStatut;
        [ObservableProperty] private bool isBusy;

        public List<StatutIntervention> Statuts { get; }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                var data = await _api.GetAllAsync(page: 1, pageSize: 100);
                _allItems = data?.ToList() ?? new List<InterventionDto>();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public Task RefreshAsync() => LoadAsync();

        partial void OnSearchTextChanged(string? value) => ApplyFilter();
        partial void OnSelectedStatutChanged(StatutIntervention? value) => ApplyFilter();

        private void ApplyFilter()
        {
            IEnumerable<InterventionDto> q = _allItems;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var txt = SearchText.Trim();
                q = q.Where(i =>
                    (i.Description?.Contains(txt, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            if (SelectedStatut.HasValue)
            {
                q = q.Where(i => i.Statut == SelectedStatut.Value);
            }

            Items = q.OrderByDescending(i => i.DatePrevue ?? i.DateRealisation ?? DateTime.MinValue)
                     .ToList();
        }

        [RelayCommand]
        public Task GoToDetails(Guid id)
            => Shell.Current.GoToAsync($"intervention-details?id={id}");
    }
}
