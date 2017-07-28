<?php
header("Content-type: image/png");
require("config.php");
    $global = new Data($_GET['user'],$_GET['pass'],"ead/");
    $img = imagecreatefromstring($global->getData(@$_GET['i']));


    echo imagepng($img);
?>