﻿// Hyphenate STring
function hyphenateString(inputString) {
    // Replace spaces with hyphens using a regular expression
    return inputString.replace(/\s+/g, '-');
}
// get the chart's colors from the data-colors attribute

function getChartColorsArray(e) {
    if (null !== document.getElementById(e))
        return (
            (e = document.getElementById(e).getAttribute("data-colors")),
            (e = JSON.parse(e)).map(function (e) {
                var t = e.replace(" ", "");
                return -1 === t.indexOf(",")
                    ? getComputedStyle(document.documentElement).getPropertyValue(t) || t
                    : 2 == (e = e.split(",")).length
                        ? "rgba(" + getComputedStyle(document.documentElement).getPropertyValue(e[0]) + "," + e[1] + ")"
                        : t;
            })
        );
}
var options,
    chart,
    chartDonutBasicColors = getChartColorsArray("simple_dount_chart"),
    url =
        (chartDonutBasicColors &&
            ((options = {
                series: [27.01, 20.87, 33.54, 37.58],
                chart: { height: 330, type: "donut" },
                legend: { position: "bottom" },
                labels: ["Documents", "Media", "Others", "Free Space"],
                dataLabels: { dropShadow: { enabled: !1 } },
                colors: chartDonutBasicColors,
            }),
                (chart = new ApexCharts(document.querySelector("#simple_dount_chart"), options)).render()),
            "../Data/"),
    allFileList = "",
    editFlag = !1,

    getJSON = function (e, t) {
        var l = new XMLHttpRequest();
        l.open("GET", url + e, !0),
            (l.responseType = "json"),
            (l.onload = function () {
                var e = l.status;
                t(200 === e ? null : e, l.response);
            }),
            l.send();
    };

function loadFileData(e) {
    document.querySelector("#file-list").innerHTML = "";

    Array.from(e).forEach(function (e, t) {
        var l = e.NomDocFile.includes(".") ?
            (function (e) {
                switch (e) {
                    case "png":
                    case "jpg":
                        return '<i class="ri-gallery-fill align-bottom text-success"></i>';
                    case "pdf":
                        return '<i class="ri-file-pdf-fill align-bottom text-danger"></i>';
                    default:
                        return '<i class="ri-file-text-fill align-bottom text-secondary"></i>';
                }
            })(e.NomDocFile.split(".")[1]) :
            '<i class="ri-folder-2-fill align-bottom text-warning"></i>';

        var i = e.starred ? "active" : "";

        document.querySelector("#file-list").innerHTML +=
            '<tr>\
                <td>\
                    <input class="form-control filelist-id" type="hidden" value="' +
            e.IdDoc +
            '" id="filelist-' +
            e.IdDoc +
            '">\
                    <div class="d-flex align-items-center">\
                        <div class="flex-shrink-0 fs-17 me-2 filelist-icon">' +
            l +
            '</div>\
                        <div class="flex-grow-1 filelist-name">' +
            e.Titre +
            '</div>\
                        <div class="d-none filelist-type">' +
            e.Format +
            '</div>\
                    </div>\
                </td>\
                <td>' +
            e.Format +
            '</td>\
                <td class="filelist-size">' +
            e.Taille +
            '</td>\
                <td class="filelist-create">' +
            e.DateUpload +
            '</td>\
                <td>\
                    <div class="d-flex gap-3 justify-content-center">\
                        <button type="button" class="btn btn-ghost-primary btn-icon btn-sm favourite-btn ' +
            i +
            '">\
                            <i class="ri-star-fill fs-13 align-bottom"></i>\
                        </button>\
                        <div class="dropdown">\
                            <button class="btn btn-light btn-icon btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false">\
                                <i class="ri-more-fill align-bottom"></i>\
                            </button>\
                            <ul class="dropdown-menu dropdown-menu-end">\
                                <li><a class="dropdown-item viewfile-list" href="#">View</a></li>\
                                <li><a class="dropdown-item edit-list" href="#createFileModal" data-bs-toggle="modal" data-edit-id=' +
            e.IdDoc +
            ' role="button">Rename</a></li>\
                                <li class="dropdown-divider"></li>\
                                <li><a class="dropdown-item remove-list" href="#removeFileItemModal" data-id=' +
            e.IdDoc +
            ' data-bs-toggle="modal" role="button">Delete</a></li>\
                            </ul>\
                        </div>\
                    </div>\
                </td>\
            </tr>';

        favouriteBtn();
        removeSingleItem();
        editFileList();
        fileDetailShow();
    });
}

function loadFolderGroupData(e, nomCatDos, location) {
    document.querySelector(location).innerHTML = '';
    if (location == "#file-manager-menu") {
        Array.from(e).forEach(function (e) {
            console.log(e);
            var nomCategDos = e.CategorieDossier.NomCatDos;
            var nCtg = hyphenateString(nomCategDos);
            document.querySelector(location).innerHTML += 
    
            e.Dossiers.length > 0 ?
            `<li class="py-2">
                <div class="d-flex">
                    <a href="#collapse-`+nCtg+`" data-bs-toggle="collapse" class="me-2 h5 arrow-folder mb-0" aria-expanded="false" aria-controls="collapse-`+nCtg+`">
                        <i class="ri-arrow-right-s-line"></i>
                        <i class="ri-arrow-down-s-line"></i>
                    </a>
                    <a href="#" onclick="$('#collapse-`+nCtg+`').collapse('hide');" class="folder-group-link">
                        <i class="ri-folder-2-line align-bottom me-2"></i> <span class="folder-group-list-link">`+nomCategDos+`</span>
                    </a>
                </div>
                <div class="collapse ps-4" id="collapse-`+nCtg+`">
                    <ul class="sub-menu list-unstyled">
                    </ul>
                </div>
            </li>`
            :
            `<li class="py-2">
                <a href="#" style="padding-left: 1.55rem;" class="folder-group-link">
                    <i class="ri-folder-2-line align-bottom me-2"></i> <span class="folder-group-list-link">`+nomCategDos+`</span>
                </a>
            </li>`
            Array.from(e.Dossiers).forEach(function (d) {
                var selector = '#collapse-'+nCtg+' ul';
                console.log(selector);
                document.querySelector(selector).innerHTML += 
                `<li>
                    <a href="#!">`+d.NomDoss+`</a>
                </li>`
            })
                
        })
    }
    if (location == "#folder-group-data") {
        Array.from(e).forEach(function (dossiers) {
            if (dossiers.CategorieDossier.NomCatDos == nomCatDos) {
                if (dossiers.Dossiers.length == 0) {
                    document.querySelector(location).innerHTML =
                    `<div class="bg-light">
                        <div class="p-5 d-flex">
                            <div style="margin: auto;">
                                <i style="font-size: 100px;" class="ri-folder-line"></i>
                                <h5 class="text-center">Empty folder</h5>
                            </div>
                        </div>
                    </div>`
                }
                Array.from(dossiers.Dossiers).forEach(function (d) {
                    document.querySelector(location).innerHTML += 
                    `<div class="col-xxl-3 col-6 folder-card">
                        <div class="card bg-light shadow-none" id="folder-1">
                            <div class="card-body">
                                <div class="d-flex mb-1">
                                    <div class="form-check form-check-danger mb-3 fs-15 flex-grow-1">
                                        <input class="form-check-input" type="checkbox" value="" id="folderlistCheckbox_1" checked>
                                        <label class="form-check-label" for="folderlistCheckbox_1"></label>
                                    </div>
                                    <div class="dropdown">
                                        <button class="btn btn-ghost-primary btn-icon btn-sm dropdown shadow-none" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                            <i class="ri-more-2-fill fs-16 align-bottom"></i>
                                        </button>
                                        <ul class="dropdown-menu dropdown-menu-end">
                                            <li><a class="dropdown-item view-item-btn" href="javascript:void(0);">Open</a></li>
                                            <li><a class="dropdown-item edit-folder-list" href="#createFolderModal" data-bs-toggle="modal" role="button">Rename</a></li>
                                            <li><a class="dropdown-item" href="#removeFolderModal" data-bs-toggle="modal" role="button">Delete</a></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="text-center">
                                    <div class="mb-2">
                                        <i class="ri-folder-2-fill align-bottom text-warning display-5"></i>
                                    </div>
                                    <h6 class="fs-15 folder-name"> `+ d.NomDoss  + `</h6>
                                </div>
                                <div class="hstack mt-4 text-muted">
                                    <span class="me-auto"><b>`+d.NbreDocs+`</b> Files</span>
                                    <span><b>`+d.Taille+`</b> Ko</span>
                                </div>
                            </div>
                        </div>
                    </div>`;
                })
            }
        })
    }
    Array.from(document.querySelectorAll(".file-manager-menu a")).forEach(function (i) {
        i.addEventListener("click", function () {
            t = document.querySelector(".file-manager-menu a.active");
            t != null ? t.classList.remove("active") : console.log();
            i.classList.add("active");
            
        });
    }),
    Array.from(document.querySelectorAll(".folder-group-link")).forEach(function (i) {
        i.addEventListener("click", function () {
            console.log(i.querySelector('.folder-group-list-link').innerHTML);
            document.getElementById("folder-group-title").innerHTML = i.querySelector('.folder-group-list-link').innerHTML;
            getJSON("categ-dossiers.json", function (e, t) {
                null !== e ? console.log("Something went wrong: " + e) : loadFolderGroupData((allFolderGroupList = t), i.querySelector('.folder-group-list-link').innerHTML, "#folder-group-data")
            })
        });
    })
}

getJSON("categ-dossiers.json", function (e, t) {
    null !== e ? console.log("Something went wrong: " + e) : loadFolderGroupData((allFolderGroupList = t), null, "#file-manager-menu");
})


function favouriteBtn() {
    Array.from(document.querySelectorAll(".favourite-btn")).forEach(function (e) {
        e.addEventListener("click", function () {
            e.classList.contains("active") ? e.classList.remove("active") : e.classList.add("active");
        });
    });
}
favouriteBtn(),

getJSON("documents.json", function (e, t) {
    null !== e ? console.log("Something went wrong: " + e) : loadFileData((allFileList = t));
})

var createFolderForms = document.querySelectorAll(".createfolder-form");
function editFolderList() {
    Array.from(document.querySelectorAll(".folder-card")).forEach(function (l) {
        Array.from(l.querySelectorAll(".edit-folder-list")).forEach(function (e) {
            e.addEventListener("click", function (e) {
                var t = l.querySelector(".card").getAttribute("id").split("-")[1];
                t == l.querySelector(".form-check .form-check-input").getAttribute("id").split("_")[1] &&
                    ((editFlag = !0),
                        (document.getElementById("addNewFolder").innerHTML = "Save"),
                        (document.getElementById("createFolderModalLabel").innerHTML = "Folder Rename"),
                        (document.getElementById("folderid-input").value = "folder-" + t),
                        (document.getElementById("foldername-input").value = l.querySelector(".folder-name").innerHTML));
            });
        });
    });
}
Array.prototype.slice.call(createFolderForms).forEach(function (i) {
    i.addEventListener(
        "submit",
        function (e) {
            var t, l;
            i.checkValidity()
                ? (e.preventDefault(),
                    (t = document.getElementById("foldername-input").value),
                    (l = Math.floor(100 * Math.random())),
                    (folderlisthtml =
                        '<div class="col-xxl-3 col-sm-6 folder-card">        <div class="card bg-light shadow-none" id="folder-' +
                        l +
                        '">            <div class="card-body">                <div class="d-flex mb-1">                    <div class="form-check form-check-danger mb-3 fs-15 flex-grow-1">                        <input class="form-check-input" type="checkbox" value="" id="folderlistCheckbox_' +
                        l +
                        '">                        <label class="form-check-label" for="folderlistCheckbox_' +
                        l +
                        '"></label>                    </div>                    <div class="dropdown">                        <button class="btn btn-ghost-primary btn-icon btn-sm dropdown" type="button" data-bs-toggle="dropdown" aria-expanded="false">                            <i class="ri-more-2-fill fs-16 align-bottom"></i>                        </button>                        <ul class="dropdown-menu dropdown-menu-end">                            <li><a class="dropdown-item view-item-btn" href="javascript:void(0);">Open</a></li>                            <li><a class="dropdown-item edit-folder-list" href="#createFolderModal" data-bs-toggle="modal" role="button">Rename</a></li>                            <li><a class="dropdown-item" href="#removeFolderModal" data-bs-toggle="modal" role="button">Delete</a></li>                        </ul>                    </div>                </div>                <div class="text-center">                <div class="mb-2">                    <i class="ri-folder-2-fill align-bottom text-warning display-5"></i>                </div>                    <h6 class="fs-15 folder-name">' +
                        t +
                        '</h6>                </div>                <div class="hstack mt-4 text-muted">                    <span class="me-auto"><b>0</b> Files</span>                    <span><b>0</b>GB</span>                </div>            </div>        </div>    </div>'),
                    "" === t || editFlag
                        ? "" !== t &&
                        editFlag &&
                        ((l = 0),
                            (l = document.getElementById("folderid-input").value),
                            (document.getElementById(l).querySelector(".folder-name").innerHTML = t),
                            document.getElementById("addFolderBtn-close").click(),
                            (editFlag = !1),
                            (document.getElementById("addNewFolder").innerHTML = "Add Folder"),
                            (document.getElementById("createFolderModalLabel").innerHTML = "Create Folder"),
                            (document.getElementById("folderid-input").value = ""),
                            (document.getElementById("foldername-input").value = ""))
                        : (document.getElementById("folderlist-data").insertAdjacentHTML("afterbegin", folderlisthtml), document.getElementById("addFolderBtn-close").click(), editFolderList()),
                    (document.getElementById("folderid-input").value = ""),
                    (document.getElementById("foldername-input").value = ""))
                : (e.preventDefault(), e.stopPropagation()),
                i.classList.add("was-validated");
        },
        !1
    );
}),
    editFolderList();
var removeProduct = document.getElementById("removeFolderModal"),
    date =
        (removeProduct &&
            removeProduct.addEventListener("show.bs.modal", function (t) {
                document.getElementById("remove-folderList").addEventListener("click", function (e) {
                    t.relatedTarget.closest(".folder-card").remove(), document.getElementById("close-removeFoldermodal").click();
                });
            }),
            new Date().toUTCString().slice(5, 16));
function editFileList() {
    var l;
    Array.from(document.querySelectorAll(".edit-list")).forEach(function (t) {
        t.addEventListener("click", function (e) {
            (l = t.getAttribute("data-edit-id")),
                (allFileList = allFileList.map(function (e) {
                    return (
                        e.id == l &&
                        ((editFlag = !0),
                            (document.getElementById("addNewFile").innerHTML = "Save"),
                            (document.getElementById("createFileModalLabel").innerHTML = "File Rename"),
                            (document.getElementById("filename-input").value = e.fileName),
                            (document.getElementById("fileid-input").value = e.id)),
                        e
                    );
                }));
        });
    });
}
Array.from(document.querySelectorAll(".createFile-modal")).forEach(function (e) {
    e.addEventListener("click", function (e) {
        (document.getElementById("addNewFile").innerHTML = "Create"),
            (document.getElementById("createFileModalLabel").innerHTML = "Create File"),
            (document.getElementById("filename-input").value = ""),
            (document.getElementById("fileid-input").value = ""),
            document.getElementById("createfile-form").classList.remove("was-validated");
    });
}),
    Array.from(document.querySelectorAll(".create-folder-modal")).forEach(function (e) {
        e.addEventListener("click", function (e) {
            (document.getElementById("addNewFolder").innerHTML = "Add Folder"),
                (document.getElementById("createFolderModalLabel").innerHTML = "Create Folder"),
                (document.getElementById("folderid-input").value = ""),
                (document.getElementById("foldername-input").value = ""),
                document.getElementById("createfolder-form").classList.remove("was-validated");
        });
    });
var createFileForms = document.querySelectorAll(".createfile-form");
function fetchIdFromObj(e) {
    return parseInt(e.id);
}
function sortElementsById() {
    loadFileData(
        allFileList.sort(function (e, t) {
            (e = fetchIdFromObj(e)), (t = fetchIdFromObj(t));
            return t < e ? -1 : e < t ? 1 : 0;
        })
    );
}
function removeSingleItem() {
    var l;
    Array.from(document.querySelectorAll(".remove-list")).forEach(function (t) {
        t.addEventListener("click", function (e) {
            (l = t.getAttribute("data-id")),
                document.getElementById("remove-fileitem").addEventListener("click", function () {
                    var t;
                    (t = l),
                        loadFileData(
                            (allFileList = allFileList.filter(function (e) {
                                return e.id != t;
                            }))
                        ),
                        document.getElementById("close-removefilemodal").click(),
                        (document.getElementById("file-overview").style.display = "none"),
                        (document.getElementById("folder-overview").style.display = "block");
                });
        });
    });
}
function fileDetailShow() {
    var d = document.getElementsByTagName("body")[0],
        t =
            (Array.from(document.querySelectorAll(".close-btn-overview")).forEach(function (e) {
                e.addEventListener("click", function () {
                    d.classList.remove("file-detail-show");
                });
            }),
                Array.from(document.querySelectorAll("#file-list tr")).forEach(function (r) {
                    r.querySelector(".viewfile-list").addEventListener("click", function () {
                        d.classList.add("file-detail-show"), (document.getElementById("file-overview").style.display = "block"), (document.getElementById("folder-overview").style.display = "none");
                        var e = r.querySelector(".filelist-id").value,
                            t = r.querySelector(".filelist-icon i").className,
                            l = r.querySelector(".filelist-name").innerHTML,
                            i = r.querySelector(".filelist-size").innerHTML,
                            n = r.querySelector(".filelist-create").innerHTML,
                            o = r.querySelector(".filelist-type").innerHTML;
                        (document.querySelector("#file-overview .file-icon i").className = t),
                            Array.from(document.querySelectorAll("#file-overview .file-name")).forEach(function (e) {
                                e.innerHTML = l;
                            }),
                            Array.from(document.querySelectorAll("#file-overview .file-size")).forEach(function (e) {
                                e.innerHTML = i;
                            }),
                            Array.from(document.querySelectorAll("#file-overview .create-date")).forEach(function (e) {
                                e.innerHTML = n;
                            }),
                            (document.querySelector("#file-overview .file-type").innerHTML = o),
                            document.querySelector("#file-overview .remove-file-overview").setAttribute("data-remove-id", e),
                            document.querySelector("#file-overview .download-file-overview").setAttribute("data-download-id", e),
                            r.querySelector(".favourite-btn").classList.contains("active")
                                ? document.querySelector("#file-overview .favourite-btn").classList.add("active")
                                : document.querySelector("#file-overview .favourite-btn").classList.remove("active");
                    });
                }),
                !1),
        l = document.getElementsByClassName("file-manager-sidebar");
    Array.from(document.querySelectorAll(".file-menu-btn")).forEach(function (e) {
        e.addEventListener("click", function () {
            Array.from(l).forEach(function (e) {
                e.classList.add("menubar-show"), (t = !0);
            });
        });
    }),
        window.addEventListener("click", function (e) {
            document.querySelector(".file-manager-sidebar").classList.contains("menubar-show") && (t || document.querySelector(".file-manager-sidebar").classList.remove("menubar-show"), (t = !1));
        });
}
function removefileOverview() {
    Array.from(document.querySelectorAll(".remove-file-overview")).forEach(function (t) {
        t.addEventListener("click", function (e) {
            var l = t.getAttribute("data-remove-id");

            document.getElementById("remove-fileitem").addEventListener("click", function () {
                var t = l;

                $.get("/Document/DeleteDoc/" + t, function (data) {
                    Swal.fire({
                        title: "Supprimé!",
                        text: "Le document a été supprimé.",
                        icon: "success",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        buttonsStyling: false
                    });
                    // Additional logic or function calls after successful deletion
                    // loadFileData(...);
                });
            });

            document.getElementById("trash-fileitem").addEventListener("click", function () {
                var t = l;

                $.get("/Document/SendToTrash/" + t, function (data) {
                    Swal.fire({
                        title: "Supprimé!",
                        text: "Le document a été envoyé dans la corbeille.",
                        icon: "success",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        buttonsStyling: false
                    });
                    location.reload();
                    // Additional logic or function calls after successful deletion
                    // loadFileData(...);
                });
            });

            document.getElementById("close-removefilemodal").click();
            document.getElementsByTagName("body")[0].classList.remove("file-detail-show");
        });
    });
}

function downloadfileOverview() {
    var l;
    Array.from(document.querySelectorAll(".download-file-overview")).forEach(function (t) {
        t.addEventListener("click", function (e) {
            (l = t.getAttribute("data-download-id")),
                /*var t;*/
                /*(t = l),*/
                console.log(l);
                Swal.fire({
                    title: "Êtes-vous sûr?",
                    text: "Vous êtes sur le point de télécharger ce document sur votre appareil.",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
                    cancelButtonClass: "btn btn-danger w-xs mt-2",
                    confirmButtonText: "Oui, télécharger!",
                    buttonsStyling: false,
                    showCloseButton: true
                }).then(function (result) {
                    if (result.value) {
                        // logic for the deletion
                        $.get("/Document/DownloadDoc/" + l, function (data) {
                            //Swal.fire({
                            //    title: "Supprimé!",
                            //    text: "Le document a été supprimé.",
                            //    icon: "success",
                            //    confirmButtonClass: "btn btn-primary w-xs mt-2",
                            //    buttonsStyling: false
                            //});
                        })
                    }
                });
                document.getElementById("remove-fileitem").addEventListener("click", function () {
                    
                        
                });
        });
    });
}
function windowResize() {
    document.documentElement.clientWidth < 1400 ? document.body.classList.remove("file-detail-show") : document.body.classList.add("file-detail-show");
}
Array.prototype.slice.call(createFileForms).forEach(function (n) {
    n.addEventListener(
        "submit",
        function (e) {
            var t, l, i;
            n.checkValidity()
                ? (e.preventDefault(),
                    (t = document.getElementById("filename-input").value),
                    (l = Math.floor(100 * Math.random())),
                    "" === t || editFlag
                        ? "" !== t &&
                        editFlag &&
                        ((i = 0),
                            (i = document.getElementById("fileid-input").value),
                            (allFileList = allFileList.map(function (e) {
                                return e.id == i ? { id: e.id, fileName: document.getElementById("filename-input").value, filetype: e.filetype, fileItem: e.fileItem, fileSize: e.fileSize, date: e.date, starred: e.starred } : e;
                            })),
                            (editFlag = !1),
                            document.getElementById("addFileBtn-close").click())
                        : (allFileList.push({ id: l, fileName: t + ".txt", filetype: "Documents", fileItem: "01", fileSize: "0 KB", date: date, starred: !1 }), sortElementsById(), document.getElementById("addFileBtn-close").click()),
                    loadFileData(allFileList),
                    (document.getElementById("addNewFile").innerHTML = "Create"),
                    (document.getElementById("createFileModalLabel").innerHTML = "Create File"))
                : (e.preventDefault(), e.stopPropagation()),
                n.classList.add("was-validated");
        },
        !1
    );
}),
    removefileOverview(),
    downloadfileOverview(),
    windowResize(),
    window.addEventListener("resize", windowResize);
