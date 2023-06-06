$.ajax({
    url: "/Utiliz/GetData",
    type: 'GET',
    success: function (data) {
        console.log(data); // Output: "Ziggy Rafiq Blog Post"
    },
    error: function (xhr, status, error) {
        console.log("Erreur")
    }
});