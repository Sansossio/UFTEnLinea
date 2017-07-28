<?php
class Data{
    var $username;
    var $password;
    var $callback;
    
    function __construct($user,$pass,$cb){
        $this->username = $user;
        $this->password = $pass;
        $this->callback = $cb;
    }
    
    public function getData($img = ""){
        $username = $this->username;
        $password = $this->password;
        $dir = "ctmp";
        $path = ($dir);
        $url="http://saia.uft.edu.ve/ead/login/index.php"; 
        $postinfo = "username=".$username."&password=".$password;

        $cookie_file_path = $path."/cookie.txt";

        $ch = curl_init();
        curl_setopt($ch, CURLOPT_HEADER, false);
        curl_setopt($ch, CURLOPT_NOBODY, false);
        curl_setopt($ch, CURLOPT_URL, $url);
        curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);

        curl_setopt($ch, CURLOPT_COOKIEJAR, $cookie_file_path);
        curl_setopt($ch, CURLOPT_COOKIE, "cookiename=0");
        curl_setopt($ch, CURLOPT_USERAGENT,
            "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US; rv:1.7.12) Gecko/20050915 Firefox/1.0.7");
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($ch, CURLOPT_REFERER, $_SERVER['REQUEST_URI']);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
        curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 0);

        curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
        curl_setopt($ch, CURLOPT_POST, 1);
        curl_setopt($ch, CURLOPT_POSTFIELDS, $postinfo);
        curl_exec($ch);
        $cls = $img == "" ? "http://saia.uft.edu.ve/$this->callback" : $img;
        curl_setopt($ch, CURLOPT_URL, $cls);
        $html = curl_exec($ch);
        curl_close($ch);
        return $html;
    }
}
?>