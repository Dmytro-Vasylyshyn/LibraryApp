document.querySelectorAll('.delete-button').forEach(button => {
    button.addEventListener('click', function (e) {
        if (!confirm('Ви впевнені, що хочете видалити цей елемент?')) {
            e.preventDefault();
        }
    });
});
