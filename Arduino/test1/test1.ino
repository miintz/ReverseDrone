#define SET(x,y) (x |=(1<<y)) //-Bit set/clear macros
#define CLR(x,y) (x &= (~(1<<y))) // |
#define CHK(x,y) (x & (1<<y)) // |
#define TOG(x,y) (x^=(1<<y)) //-+

volatile unsigned char inptr = 0;
volatile unsigned char ipr = 0;
volatile unsigned char outptr = 0;
volatile unsigned char data[256];
volatile unsigned char line[256];
volatile unsigned char frame[256];

volatile unsigned char buffer[256];
volatile unsigned char linebuf[256];

SIGNAL(INT0_vect)
{
  TOG(PORTD, 7);
  inptr = ipr;
  frame[(ipr - 1)] = 1;
}

SIGNAL(SPI_STC_vect)
{
  TOG(PORTD, 7);
  data[ipr] = SPDR;
  line[ipr] = PIND;
  frame[ipr] = 0;
  ipr++;
  // inptr=ipr;
}

void setup()
{
  Serial.begin(115200);
  SPCR = 0b11000000;
  EICRA = 0b00000011;
  SET(EIMSK, INT0);
  SET(DDRD, 7);
  TIMSK1 = 0;
  TIMSK0 = 0;
  TIMSK2 = 0;
  sei();
  Serial.println("Log:");
  Serial.println("\'.\'=read status");
  Serial.println("\'N\'=NOP");
  Serial.println("\'Z\'=Clear flags ");
}

unsigned char ls = 0;

unsigned char dptr = 0;



void loop()
{
  while (inptr != outptr)
  {
    outptr++;

    buffer[dptr] = data[outptr];
    linebuf[dptr++] = line[outptr];


    if (frame[outptr] == 1) //-Frame
    {
      //***************************************************
      // Handle frame
      //***************************************************

      /* for(unsigned char i=0;i<dptr;i++)
      {
      Serial.print("0X");
      if(buffer[i]<0x10)
      Serial.print("0");
      Serial.print(buffer[i],HEX);
      Serial.print(",");
      }
      Serial.println();
      */

      switch (buffer[0])
      {


        // case 0x20:
        // case 0x61:
        case 0xa0:
          // case 0x2A:
          // case 0x30:
          Serial.print(" ");
        case 0x25:
          // case 0xA0:
          for (unsigned char i = 0; i < dptr; i++)
          {
            Serial.print("0X");
            if (buffer[i] < 0x10)
              Serial.print("0");
            Serial.print(buffer[i], HEX);
            Serial.print(",");
          }
          Serial.println();
          break;
        /*
        case 0x20:
        if(buffer[1]&&0x01)
        Serial.println("TX");
        else
        Serial.println("RX");
        break;
        */
        default:
          break;
      }

      for (unsigned int i = 0; i < 300; i++)
        asm("nop");
      //***************************************************
      dptr = 0;
    }
  }
}
