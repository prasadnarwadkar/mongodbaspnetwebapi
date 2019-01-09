<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="apitest.aspx.cs" Inherits="MongoDbASPNetWebAPI.apitest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            postValueToWebAPI();
        });

        function postValueToWebAPI() {
            var val = "gates";
            var hero = {};
            //hero.id = 337;
            hero.name = val;

            alert(JSON.stringify(hero));
            $.ajax({
                type: "POST",
                url: "api/ourheroes",
                data: hero,

                dataType: 'application/json'
            });

            location.href = 'https://mongodbwebapi.azurewebsites.net/api/ourheroes';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
