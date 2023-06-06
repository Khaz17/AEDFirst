document.addEventListener("DOMContentLoaded", function () {
    new DataTable("#users-table", {
        fixedHeader: !0,
        columnDefs: [
            { orderable:false, targets:[0,8] }
        ]
    })
})


$(document).ready(function () {
    // check/uncheck all checkboxes when the table header checkbox is clicked
    $("#checkall").on("click", function () {
        $(".checkone").prop("checked", $(this).prop("checked"));
        $('')
    });
});


    //var checkAll = document.getElementById("checkAll")
    //    , perPage = (checkAll && (checkAll.onclick = function () {
    //        for (var e = document.querySelectorAll('.form-check-all input[type="checkbox"]'), t = document.querySelectorAll('.form-check-all input[type="checkbox"]:checked').length, a = 0; a < e.length; a++)
    //            e[a].checked = this.checked,
    //                e[a].checked ? e[a].closest("tr").classList.add("table-active") : e[a].closest("tr").classList.remove("table-active");
    //        document.getElementById("remove-actions").style.display = 0 < t ? "none" : "block"
    //    }
    //    ),
    //        8)


    