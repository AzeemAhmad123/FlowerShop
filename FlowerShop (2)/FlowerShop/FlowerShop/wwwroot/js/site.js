// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Dark Mode Functionality
(function () {
    // Function to get cookie value
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    }

    // Function to apply dark mode
    function applyDarkMode(isDark) {
        if (isDark) {
            document.body.classList.add('dark-mode');
        } else {
            document.body.classList.remove('dark-mode');
        }
    }

    // Make applyDarkModePreview globally accessible for inline event handlers
    window.applyDarkModePreview = function (isDark) {
        applyDarkMode(isDark);
    };

    // Check cookie and apply dark mode on page load
    function initDarkMode() {
        const darkModeCookie = getCookie('IsDarkMode');
        const isDarkFromCookie = darkModeCookie === 'true';

        // Apply dark mode based on cookie
        applyDarkMode(isDarkFromCookie);
    }

    // Initialize dark mode when page loads
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () {
            initDarkMode();
            setupToggleListener();
        });
    } else {
        initDarkMode();
        setupToggleListener();
    }

    // Setup toggle listener
    function setupToggleListener() {
        const darkModeToggle = document.getElementById('DarkModeToggle');
        if (darkModeToggle) {
            // Apply dark mode immediately when toggle changes
            darkModeToggle.addEventListener('change', function () {
                const isDark = this.checked;
                applyDarkMode(isDark);
            });
        }
    }
})();