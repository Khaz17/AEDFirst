$(document).ready(function () {
    // Event handler for the button or link that triggers the modal
    $('#openCreateCDModalButton').click(function () {
        // Load the partial view content using AJAX
        $.get('/CategorieDossier/CreateCatDossier', function (data) {
            // Update the modal body with the partial view content
            $('#createCDModal .modal-body').html(data);

            // Show the modal
            $('#createCDModal').modal('show');
        });
    });

    $('.openEditCDModalButton').click(function () {
        var id = $(this).data("id");
        // Load the partial view content using AJAX
        $.get('/CategorieDossier/EditCatDossier/' + id, function (data) {
            // Update the modal body with the partial view content
            $('#editCDModal .modal-body').html(data);

            // Show the modal
            $('#editCDModal').modal('show');
        });
    });

    $(".deleteBtn").on("click", function () {
        var id = $(this).data("id");
        var nbredossiers = $(this).data("nbredossiers")
        var nomcat = $(this).closest("h5").text()
        if (nbredossiers == 0) {
            Swal.fire({
                title: "Êtes-vous sûr?",
                text: "Cette catégorie est vide.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
                cancelButtonClass: "btn btn-danger w-xs mt-2",
                confirmButtonText: "Oui, supprimer!",
                buttonsStyling: false,
                showCloseButton: true
            }).then(function (result) {
                if (result.value) {
                    // logic for the deletion
                    $.post('/CategorieDossier/DeleteCatDossier/' + id, function (data) {
                        Swal.fire({
                            title: "Supprimé!",
                            text: "La catégorie " + nomcat + " a été supprimée.",
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            location.reload()
                        }, 5000)
                    })
                }
            });
        } else if (nbredossiers >= 0) {
            Swal.fire({
                title: "Supprimer la catégorie?",
                text: "Cette catégorie comprend " + nbredossiers + " dossier(s). Que voulez-vous faire ?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Tout vider",
                cancelButtonText: "Garder les dossiers et leur contenu",
                cancelButtonColor: "#3085d6",
                cancelButtonClass: "btn btn-secondary",
                showCloseButton: true,
                reverseButtons: true,
                focusCancel: true,
                //customClass: {
                //    confirmButton: "btn btn-danger",
                //},
                buttonsStyling: false,
            }).then(function (result) {
                if (result.isConfirmed) {
                    $.get('/CategorieDossier/DeleteCatDossierRecursive/' + id, function (data) {
                        Swal.fire({
                            title: "Supprimé!",
                            text: "La catégorie " + nomcat + " et son contenu ont été supprimés.",
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            location.reload()
                        }, 5000)
                    })
                    // "Tout vider" button clicked
                    // Perform the action to delete all data
                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    // "Garder les dossiers et leur contenu" button clicked
                    // Perform the action to keep the folders and their content
                    $.get('/CategorieDossier/DeleteCatDossier/' + id, function (data) {
                        Swal.fire({
                            title: "Supprimé!",
                            text: "La catégorie " + nomcat + " a été supprimée.",
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            location.reload()
                        }, 5000)
                    })
                } else {
                    // "Annuler" or close button clicked
                    // No action needed or perform additional actions
                }
            });

        }
    });
});