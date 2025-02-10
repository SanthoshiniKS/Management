// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    var toolTipTrigger = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var toolTipList = toolTipTrigger.map(function (toolTipTriggerE1) {
        return new bootstrap.Tooltip(toolTipTriggerE1);
    });
});
