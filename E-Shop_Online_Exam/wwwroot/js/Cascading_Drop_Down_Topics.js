$("#cert").change(function () {
    $("#topic").empty();
    $.ajax
        ({
            type: "Post",
            url: "/Question/GetTopics",
            data: { "id": $("#cert").val() },
            success: function (response) {
                var items = '';
                $(response).each(function () {
                    items = this.text;
                    var x = document.getElementById("topic");
                    var option = document.createElement("option");
                    option.text = items;
                    option.value = this.value;
                    x.add(option);
                })
            },
            failure: function (response) {
                alert(repsonse.repsonseText);
            },
            error: function (response) {
                alert(repsonse.repsonseText);
            }
        });
});

