<?php
    $hostname = "localhost";
    $database = "dbpm02examen";
    $username = "root";
    $password = "";

    if(isset($_GET["id"]))
    {
        $codigo=$_GET["id"];
        $conexion=mysqli_connect($hostname, $username, $password, $database);

        $insertar = "DELETE FROM `sitios` WHERE `id`=$codigo";
        $resultado=mysqli_query($conexion, $insertar);

        $json[]="Exitoso";

        echo json_encode($json);
    }
?>