<?php
header('Content-Type: application/json');
require("config.php");
$global = new Generate(@$_GET['user'],@$_GET['pass']);
echo $global->getJSON();
?>
