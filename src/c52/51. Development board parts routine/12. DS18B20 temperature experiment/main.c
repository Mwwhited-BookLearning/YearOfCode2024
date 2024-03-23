/********************************* �����к�̫�������޹�˾ *******************************
* ʵ �� �� ��DS18B20�¶�ʵ��
* ʵ��˵�� ���������ʾ��ǰ�¶�
* ʵ��ƽ̨ ����̫51��Ƭ��������
* ���ӷ�ʽ ����DS18B20�嵽�����¶ȼ��ģ���U5��  DS_RD��P22
* ע    �� ��
* ��    �� ����̫���Ӳ�Ʒ�з���    QQ ��1909197536
* ��    �� ��http://shop120013844.taobao.com/
****************************************************************************************/
#include <reg52.h>
#include <intrins.h>

#define uchar unsigned char
#define uint  unsigned int

#define FOSC 11059200L //�������ã�Ĭ��ʹ��11.0592M Hz
//#define FOSC 12000000L //�������ã�ʹ��12M Hz
//#define FOSC 24000000L //�������ã�ʹ��24M Hz

//IO�ӿڶ���
sbit DQ = P2^2;  //����DS18B20�˿�DQ  
#define LED_PORT P0
sbit wela_1 = P2^4;
sbit wela_2 = P2^5;
sbit wela_3 = P2^6;
sbit wela_4 = P2^7;

unsigned char temperature=0;
//LED��ʾ��ģ 0-F ����ģʽ
unsigned code table[]= {0Xc0,0xf9,0xa4,0xb0,0x99,0x92,0x82,0xf8,0x80,0x90,0x88,0x83,0xc6,0xa1,0x86,0x8e};
unsigned char RomCode[8];
/*******************************************************************************
* �� �� �� ��Delayms
* �������� ��ʵ�� ms������ʱ
* ��    �� ��ms
* ��    �� ����
*******************************************************************************/
void Delayms(unsigned int ms)
{
	unsigned int i,j;
	for(i=0;i<ms;i++)
	#if FOSC == 11059200L
		for(j=0;j<114;j++);
	#elif FOSC == 12000000L
	  for(j=0;j<123;j++);
	#elif FOSC == 24000000L
		for(j=0;j<249;j++);
	#else
		for(j=0;j<114;j++);
	#endif
}

/*******************************************************************************
* �� �� �� ��LEDdisplay
* �������� ��ѭ����ʾ����λ�ϵ�����
* ��    �� ��numҪ��ʾ������
* ��    �� ����
*******************************************************************************/
void LEDdisplay(unsigned int num)
{
	unsigned char qian,bai,shi,ge;
	qian=num/1000;
	bai=num%1000/100;
	shi=num%100/10;
	ge=num%10;
	
	wela_1 = 1;	  //�ر����������
	wela_2 = 1;
	wela_3 = 1;
	wela_4 = 1;	

	wela_4=0;  //��ʾǧλ
	LED_PORT=table[qian];
	Delayms(1);
	LED_PORT = 0xff;
	wela_4=1;
	
	wela_3=0;  //��ʾ��λ
	LED_PORT=table[bai];
	Delayms(1);
	LED_PORT = 0xff;
	wela_3=1;
	
	wela_2=0;  //��ʾʮλ
	LED_PORT=table[shi];
	Delayms(1);
	LED_PORT = 0xff;
	wela_2=1;
	
	wela_1=0;  //��ʾ��λ
	LED_PORT=table[ge];
	Delayms(1);
	LED_PORT = 0xff;
}

/*******************************************************************/
/*                                                                 */
/*us����ʱ����                                                     */
/*                                                                 */
/*******************************************************************/

void Delay(unsigned int num)
{
  while( --num );
}

/*******************************************************************/
/*                                                                 */
/*��ʼ��ds1820                                                     */
/*                                                                 */
/*******************************************************************/
bit Init_DS18B20(void)
{ 
		bit presence;
     DQ = 1;      //DQ��λ
     Delay(8);    //������ʱ

     DQ = 0;      //��Ƭ����DQ����
     Delay(90);   //��ȷ��ʱ ���� 480us

     DQ = 1;       //��������
     Delay(8);

     presence = DQ;    //���=0���ʼ���ɹ� =1���ʼ��ʧ��
     Delay(100);
     DQ = 1; 
     
     return(presence); //�����źţ�0=presence,1= no presence
}

/*******************************************************************/
/*                                                                 */
/* ��һ���ֽ�                                                      */
/*                                                                 */
/*******************************************************************/
unsigned char ReadOneChar(void)
{
   unsigned char i = 0;
   unsigned char dat = 0;

   for (i = 8; i > 0; i--)
   {
     DQ = 0; // �������ź�
     dat >>= 1;
     DQ = 1; // �������ź�

     if(DQ)
     dat |= 0x80;
     Delay(4);
   }
    return (dat);
}

/*******************************************************************/
/*                                                                 */
/* дһ���ֽ�                                                      */
/*                                                                 */
/*******************************************************************/
void  WriteOneChar(unsigned char dat)
{
  unsigned char i = 0;
  for (i = 8; i > 0; i--)
  {
    DQ = 0;
    DQ = dat&0x01;
    Delay(5);

    DQ = 1;
    dat>>=1;
  }
}

/*******************************************************************/
/*                                                                 */
/* �¶ȱ���ֵд��DS18B20                                           */
/*                                                                 */
/*******************************************************************/
void Write_Temperature_alarm(unsigned char Temp_h , unsigned char Temp_l)
{
    Init_DS18B20();
  
    WriteOneChar(0xCC);           //����������кŵĲ��� 
    WriteOneChar(0x4e);           //���趨���¶ȱ���ֵд�� DS18B20
	WriteOneChar(Temp_h);  //дTH
	WriteOneChar(Temp_l);  //дTL
    WriteOneChar(0x7f);           //12λ��ȷ��
 
	Init_DS18B20();
    WriteOneChar(0xCC);           //����������кŵĲ��� 
    WriteOneChar(0x48);           //���ݴ�������¶ȱ���ֵ������EEROM
}

/*******************************************************************/
/*                                                                 */
/* ��ȡ64λ������                                                  */
/*                                                                 */
/*******************************************************************/
void Read_RomCord(void)
{
   unsigned char j;
   Init_DS18B20();
  
   WriteOneChar(0x33);          // ��������Ĳ���
   for (j = 0; j < 8; j++)
   {
     RomCode[j] = ReadOneChar() ; 
   }
}

/*******************************************************************/
/*                                                                 */
/*DS18B20��CRC8У�����                                            */
/*                                                                 */
/*******************************************************************/
unsigned char CRC8() 
{ 
   unsigned char i,x; 
	unsigned char crcbuff,crc;
   
   crc=0;
   for(x = 0; x <8; x++)
   {
    crcbuff=RomCode[x];
    for(i = 0; i < 8; i++) 
     { 
      if(((crc ^ crcbuff)&0x01)==0) 
      crc >>= 1; 
       else { 
              crc ^= 0x18;   //CRC=X8+X5+X4+1
              crc >>= 1; 
              crc |= 0x80; 
            }         
      crcbuff >>= 1;       
	 }
   }
     return crc;	
}

/*******************************************************************/
/*                                                                 */
/* ��ȡ�¶�                                                        */
/*                                                                 */
/*******************************************************************/
unsigned char Read_Temperature(void)
{
	unsigned char  i; 
	unsigned char temp_comp;	
	unsigned char temp_data[2];
	unsigned char temp_alarm[2];
	Init_DS18B20();

	WriteOneChar(0xCC);        //����������кŵĲ���
	WriteOneChar(0x44);        //�����¶�ת��

	Init_DS18B20();

	WriteOneChar(0x55);         //ƥ��ROM����
	for(i=0;i<8;i++)
	WriteOneChar(RomCode[i]);

	WriteOneChar(0xBE);         //��ȡ�¶ȼĴ���

	temp_data[0] = ReadOneChar();   //�¶ȵ�8λ
	temp_data[1] = ReadOneChar();   //�¶ȸ�8λ
	temp_alarm[0] = ReadOneChar();  //�¶ȱ���TH
	temp_alarm[1] = ReadOneChar();  //�¶ȱ���TL

	temp_comp=((temp_data[0]&0xf0)>>4)|((temp_data[1]&0x0f)<<4);//ȡ�¶�����ֵ
	return temp_comp;  														 
}

void main(void)
{
	int count = 0;
	Read_RomCord();
	while(1)
	{
		if(count >= 100)
		{
			temperature = Read_Temperature();
			count = 0;
		}
		LEDdisplay(temperature);
		count++;
	}
}

