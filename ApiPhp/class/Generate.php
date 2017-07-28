<?php
class Generate{
    private $user,$pass;
    function __construct($user,$pass){
        $this->user = $user;
        $this->pass = $pass;
    }
    private function getData($var,$file){
        $global = new Saia($this->user,$this->pass,$file);
        return $var == "" ? $global->getUrl() : $global->getName($var);
    }
    private function getCourses(){
        
        $courses = str_get_html($this->getData(".minicalendar","ead/"));
        $tmp = [];
        $c = 0;
        $dayss = [];
        $accptD = [];
        $ad = date("d");
        for($i = 1; $i <= 8; $i++){
            $nd = $ad + $i;
            if($nd > 31){
                $ad = 0;
                $nd = 1;
            }
            array_push($accptD, $nd);
        }
        foreach($courses->find('td[data-core_calendar-popupcontent]') as $element){
            $tag = 'data-core_calendar-popupcontent';
            $dday = $element->plaintext;
            if($element->{$tag} != "No hay eventos" && in_array($dday, $accptD) ){
                $exist = [];
                $html = str_get_html(html_entity_decode($element->{$tag}));
                $url = "";
                $tarea = "";
                
                foreach($html->find('a') as $sa){
                    $url = str_replace("http://saia.uft.edu.ve/","",$sa->href);
                    $url = str_replace('&amp;','&', $url);
                    $tar = str_get_html($this->getData(".eventlist",$url));
                    foreach($tar->find('.event') as $das){
                            $hs = str_get_html($das);
                            foreach($hs->find('.course') as $da3)
                                $title = "($dday" . date("/m/Y") . ") " . $da3->plaintext;
                            
                            foreach($hs->find('.description') as $des)
                                $desc = $des->plaintext;
                            
                            foreach($hs->find('.referer') as $num)
                                $num = $num->plaintext;
                                
                            
                            if(!isset($exist[$num . $title])){
                                $tarea .= "ZZY{$title}XX{$desc}XX{$num}";
                                $exist[$num . $title] = $num;
                            }
                        
                    }
                }
                
                if(in_array($dday, $accptD)){
                    $data["day"] = $dday;
                    $data["tarea"] = substr($tarea,3);
                    $tmp[$c] = $data;
                }
                
                $c++;
            }     
        }
        return $tmp;
    }
    public function getJSON(){
        $result["name"] = $this->getData("div.fullname","ead/");
        $result["picture"] = $this->getData("","ead/");
        $result["courses"] = $this->getCourses();
        $result["error"] = "none";
        
        //print_r($result); 
        return json_encode($result);
    }
}
?>