#import	"Utility.h"

typedef char	Char;
typedef unsigned char	UChar;

typedef short	Short;
typedef unsigned short	UShort;
typedef unsigned short	Ushort;

typedef int	Int;
typedef unsigned int	UInt;

typedef long	Long;
typedef unsigned long	ULong;

typedef short	Sint;
typedef unsigned short	Suit;

typedef UChar	uch;
typedef UShort	ush;

typedef float   Float;
typedef UChar	SByte;

typedef struct _BCR_Engine
{
    void		*pBcrEngine;
    
    Char		pBuffer[4096];
}
BEngine;

typedef struct T_Rect
{
    Short	lx;
    Short	ly;
    Short	rx;
    Short	ry;
}
TRect;

typedef struct B_Rect
{
    Short	lx;
    Short	ly;
    Short	rx;
    Short	ry;
}
BRect;

typedef struct T_MastImage {
    
    Short	width;
    Short	height;
    Short	xres;
    Short	yres;
    SByte	**pixels;
    
    UShort	type;
    UChar	status;
    UChar	key;
    
    Short x1, x2;	//
    Short y1, y2;	//
    TRect CropRect;//added by CZ for getting exact position in the end.20090205
    void  *ptr;		// can be used to pass user defined struct
    
    struct T_MastImage	*secImage;
    
    SByte	msk[24];
}TMastImage;

typedef TMastImage	SCAN_IMAGE;
typedef TMastImage	OCR_IMAGE;
typedef TMastImage	BImage;
typedef TMastImage	TImage;

typedef struct B_Field
{
    //	union
    //	{
    Short	fid;		// Field ID,for BCR result
    Short   blocktype;  //for OCR result
    //	};
    Short	mem;
    
#ifdef __IDCR
    Int     cardtype;
    BLine *pline;
    Char *poritext; //save originally text
    BRect headImgRect;     //the position of head image on id_card image * tjh add 20111208
#endif
    
    Char	*text;		// Field text
    int     conf;		// Field conf
    int     addconf;		// Field conf
    BRect	rect;		// Field position on image
    BRect	rectint;	// Field position on internal image
    Short	label;
    Short	size;		// Field text buffer size
    
#ifdef _HC_POSITION
    Int		nchars;
    BChar	*pchars;
    Int     angle;
#endif
    
#ifdef WITH_PINYIN
    Char	*textpy;		// Field text
#endif
    
    
#ifdef FONT_TYPE
    UChar *pTraFont;  //for T_CH case: one GB code accord with muti GIG5 codes
    Font_Type iFontType;
#endif
    
    Char   reserve[32];
    
    struct B_Field	*child;
    struct B_Field	*prev;
    struct B_Field	*next;
}
BField;

//for OCR language definition
typedef enum {
    
    OCR_LAN_NIL				= 0x00,	// NIL
    OCR_LAN_ENGLISH			= 0x01,	// English
    OCR_LAN_CHINESE			= 0x02,	// chinese
    OCR_LAN_EUROPEAN		= 0x03,	// English + European
    OCR_LAN_RUSSIAN 		= 0x04,	// Russian
    OCR_LAN_TAMIL	 		= 0x05,	// Tamil
    OCR_LAN_JAPAN 			= 0x06,	// Japan
    OCR_LAN_CENTEURO        = 0x07, // Central European
    OCR_LAN_KOREA           = 0x08, // KOREA
    OCR_LAN_TURKISH         = 0x09, // English + Turkish
    OCR_LAN_MixedEC			= 0x10,	// mixed chinese/english
    
    OCR_LAN_MAX	 			= 0x12
    
} OCR_Language;
