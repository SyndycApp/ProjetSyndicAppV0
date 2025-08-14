(function () {
    const KEY = "syndicapp-theme";
    const root = document.documentElement;
    function apply(mode) {
        if (mode === "dark") root.classList.add("dark");
        else root.classList.remove("dark");
    }
    const saved = localStorage.getItem(KEY);
    if (saved) apply(saved);
    document.addEventListener("click", e => {
        const t = e.target.closest("[data-theme-toggle]");
        if (!t) return;
        const isDark = root.classList.contains("dark");
        const next = isDark ? "light" : "dark";
        localStorage.setItem(KEY, next);
        apply(next);
    });
})();