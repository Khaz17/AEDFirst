$(document).ready(function () {

    $('#IdCateDos').change(function () {
        var idcategnow = $(this).val();
        $.ajax({
            url: '/CategorieDossier/GetFolders/',
            type: 'GET',
            data: { idcateg: idcategnow },
            dataType: 'json',
            success: function (data) {
                $('#IdDoss').empty();
                $('#IdDoss').append($('<option>').val('-1').text('Sélectionnez un dossier...'));
                $.each(data, function (index, folder) {
                    $('#IdDoss').append($('<option>').val(folder.IdDoss).text(folder.NomDoss));
                });
            },
            error: function () {
                // Tu gères les erreurs ici
                alert('Dossiers non trouvés')

            }
        });
    });

    $('#DocFile').change(function (event) {
        file = event.target.files[0];
        var nom = file.name.split('.').slice(0, -1).join('.');
        var size = file.size;
        var format = file.name.split('.').pop();

        $('#Titre').val(nom);
        $('#Taille').val(size);
        $('#Format').val(format);

        //var tags = $('#Tags').val();
        //console.log(tags);
    })

})