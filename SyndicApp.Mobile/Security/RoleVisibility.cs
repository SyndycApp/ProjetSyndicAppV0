using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Security
{
    public static class RoleVisibility
    {
        public static readonly BindableProperty RolesProperty =
            BindableProperty.CreateAttached(
                "Roles",
                typeof(string),
                typeof(RoleVisibility),
                default(string),
                propertyChanged: OnRolesChanged);

        public static string? GetRoles(BindableObject view)
            => (string?)view.GetValue(RolesProperty);

        public static void SetRoles(BindableObject view, string? value)
            => view.SetValue(RolesProperty, value);

        private static void OnRolesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not VisualElement element)
                return;

            UpdateVisibility(element, newValue as string);
        }

        private static void UpdateVisibility(VisualElement element, string? rolesCsv)
        {
            // Si pas de rôles -> visible pour tout le monde
            if (string.IsNullOrWhiteSpace(rolesCsv))
            {
                element.IsVisible = true;
                return;
            }

            var allowedRoles = rolesCsv
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var userRoles = UserRoleStore.Roles;

            if (!userRoles.Any())
            {
                // Aucun rôle chargé -> on peut choisir false (plus sécurisé)
                element.IsVisible = false;
                return;
            }

            element.IsVisible = userRoles.Any(r =>
                allowedRoles.Contains(r, StringComparer.OrdinalIgnoreCase));
        }

        // À appeler manuellement si tu changes les rôles après que le XAML soit déjà chargé
        public static void RefreshForElement(VisualElement element)
        {
            var rolesCsv = GetRoles(element);
            UpdateVisibility(element, rolesCsv);
        }
    }
}
