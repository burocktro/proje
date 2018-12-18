int Konum;
int Bekleme;

void setup()
{
  pinMode(8,OUTPUT);
  pinMode(9,OUTPUT);
  pinMode(10,OUTPUT);
  pinMode(11,OUTPUT);

  digitalWrite(8,LOW);
  digitalWrite(9,LOW);
  digitalWrite(10,LOW);
  digitalWrite(11,LOW);
  Konum=8;
  Bekleme=10;
  Serial.begin(9600);
}
void loop(){
if(Serial.available()){

int  gelenVeri=Serial.read();

  if(gelenVeri=='1'){

  solaDon(500);
  }

  if(gelenVeri=='3'){
    
  sagaDon(500);
  
  
  }
}
}


void KonumArttir(){
  Konum ++;
  if(Konum==12) Konum=8;
}

void KonumAzalt(){
  Konum --;
  if(Konum==7) Konum=11;
}
  


void sagaDon(int Adim){

  digitalWrite(Konum,HIGH);
  delay(Bekleme);
  digitalWrite(Konum,LOW);
  KonumArttir();
}

void solaDon(int Adim){

  digitalWrite(Konum,HIGH);
  delay(Bekleme);
  digitalWrite(Konum,LOW);
  KonumAzalt();

}
