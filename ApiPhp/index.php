<?php
header('Content-Type: application/json');
require("config.php");
// Daniel
/*
$global = new Saia("Uftpre25544190","25544190","ead/");
echo $global->getJSON();/*
// Arturo
$global = new Saia("Uftpre21126029","55554342452","ead/");
echo $global->getJSON();
// John 
$global = new Saia("Uftpre26437215","jeam26437215","ead/");
echo $global->getJSON();
// Ariana
/*
$global = new Saia("Uftpre26437248","26437248","ead/");
echo $global->getJSON();*/
$global = new Generate(@$_GET['user'],@$_GET['pass']);
echo $global->getJSON();
?>