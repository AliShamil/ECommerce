// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => {
    $(".nav a").mouseenter(function () { $(this).css("background-color", "#21B092") }).mouseleave(function () { $(this).css("background-color", "initial") });
})