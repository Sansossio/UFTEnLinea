<?php
class Saia extends Data{
    var $content;
    var $div = [];
    var $r = false;
    function __construct($user,$pass,$cb){
        parent::__construct($user,$pass,$cb);
        $this->content = (parent::getData());
        if($this->Error()){
            die(json_encode((["error" => "loginFail"])));
        }
        
    }
    // Clases privadas
    private function search($t,$x=0,$h=0){
        $html = $x == 0 ? str_get_html($this->content) : str_get_html($x);
        $r = "";
        foreach($html->find($t) as $article) { 
            if($t != ".eventlist" && $t != ".minicalendar"){
                $r .= $h == 0 ? $article->plaintext : $article->src;
            }else{
                $r .= $article;
            }
            
        }
        return $r;
    }
    private function profile(){
        return $this->search("div.myprofileitem");
    }
    private function get($class,$x=0){
        $r = "";
        $c = $this->profile();
        $html = $this->search("$class",$c,$x);
        return $html;
    }
    // Verificar error
    private function Error(){
        $this->r = false;
        $pos = strpos($this->content,"http://saia.uft.edu.ve/ead/login/index.php");
        if($pos === false){
            $this->r = false;
        }else{
            $this->r = true;
        }
        return $this->r;
    }
    // Clases publicas
    // Buscar nombres
    public function getName($find){
        return !$this->r ? $this->get($find) : "";
    }
    // Buscar foto
    public function getUrl(){
        $url = "http://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";
        return !$this->r ? ("http://comercialotodo.com/image/uft/img.php?user=" . @$_GET["user"] . "&pass=" . @$_GET["pass"] . "&i=".$this->get("img.profilepicture",1)) : "";
    }
    public function getPicture(){
        return "<img src='img.php?i=".$this->getUrl()."'>";
    }
    public function getJSON(){
        $r = [];
        $r['name'] = $this->getName();
        $r['picture'] = $this->getUrl();
        $r['error'] = 'none';
        return json_encode($r);
    }
    
}

?>