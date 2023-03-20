<?php
    $hostname = "localhost";
    $database = "dbpm02examen";
    $username = "root";
    $password = "";

    $datos=json_decode(file_get_contents("php://input"));

    if(isset($datos))
    {
        $id=$datos->id;
        $descripcion=$datos->descripcion;
        $latitud=$datos->latitud;
        $longitud=$datos->longitud;
        $firmadigital=$datos->firmadigital;
        $trazado=$datos->trazado;

        $conexion=mysqli_connect($hostname, $username, $password, $database);

        $insertar="UPDATE `sitios` SET `descripcion`='$descripcion',`latitud`='$latitud',`longitud`='$longitud',`firmadigital`='$firmadigital',`trazado`='$trazado' WHERE (`id`='$id')";

        $resultado=mysqli_query($conexion, $insertar);

        mysqli_close($conexion);
    }
    echo "Exitoso";
?>
