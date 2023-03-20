<?php
    $hostname = "localhost";
    $database = "dbpm02examen";
    $username = "root";
    $password = "";
    
    $conexion=mysqli_connect($hostname, $username, $password, $database);

    if (!$conexion) {
        die("La conexión ha fallado: " . mysqli_connect_error());
    }
    else
    {
    
    $sql = "SELECT id, descripcion, latitud, longitud, firmadigital, audiofile FROM sitios";

    $resultado = mysqli_query($conexion, $sql);

        if (mysqli_num_rows($resultado) > 0) {

        $sitios = array();
        while ($fila = mysqli_fetch_assoc($resultado)) {

            $sitios[] = $fila;
        }

        echo json_encode($sitios);

        } else {
            echo "No se encontraron resultados.";
        }

        mysqli_close($conexion);
    }
?>