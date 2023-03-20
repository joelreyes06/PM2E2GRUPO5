<?php
    $hostname = "localhost";
    $database = "dbpm02examen";
    $username = "root";
    $password = "";

    $datos=json_decode(file_get_contents("php://input"));

    if(isset($datos)) {
        $id=NULL;
        $descripcion=$datos->descripcion;
        $latitud=$datos->latitud;
        $longitud=$datos->longitud;
        $firmadigital=$datos->firmadigital;
        $audiofile=$datos->audiofile;
        $trazado=$datos->trazado;

        $conexion=mysqli_connect($hostname, $username, $password, $database);
        $insertar="INSERT INTO `sitios`(`id`, `descripcion`, `latitud`, `longitud`, `firmadigital`, `audiofile`, `trazado`) VALUES ('$id','$descripcion','$latitud','$longitud','$firmadigital','$audiofile','$trazado')";
        $resultado=mysqli_query($conexion, $insertar);
        
        echo json_encode($json);
        
        mysqli_close($conexion);
        echo "Exitoso";
    }
    
?>