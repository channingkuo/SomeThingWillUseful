//
//  OCREngine.h
//  IDCardScanDemo
//
//  Created by  on 14-04-16.
//  Copyright (c) 2011年 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "Common.h"
#define BIDC_HEADIMAGE      -10

@protocol BCRProgressCallBackDelegate;

@protocol BcrResultCallbackDelegate <NSObject>
@required -(void)bcrResultCallbackWithValue:(NSInteger)value;
@end

@interface YMIDCardEngine : NSObject
{
//    BEngine         *_ocrEngine;
    BEngine         *_bcrEngine;
    BImage          *_bImage;
    BImage          *_picImage;
    BImage          *_headBImage;
    
    CGRect          textRect;
    
    NSInteger       progress;
    UIImage         *idCardImage;
    UIImage         *headImage;
}

typedef enum
{
    ID_NON,                 //non
    ID_cardNo,              //卡号
    ID_cardName,            //姓名
    ID_addr,                //
    ID_birthday,            //
    ID_sex
} IDCard;

@property (nonatomic, assign) NSInteger ocrLanguage;
//@property (nonatomic, readonly) NSInteger codePage;
@property (nonatomic) BOOL     initSuccess;
@property(nonatomic, assign) NSString *chNumberStr;

- (id)initWithLanguage:(NSInteger)language andChannelNumber:(NSString*)channelNumberStr;

- (BOOL)allocBImage:(UIImage*)image;
- (BOOL)allocVideoBImage:(UIImage *)image;
- (void)freeBImage;
- (BOOL)rotateBImage;
//- (BOOL)blurDetection;

- (CGRect)charDetection:(CGPoint)firstPoint lastPoint:(CGPoint)lastPoint;
- (NSDictionary *)doBCRWithRect:(CGRect)rect;
- (UIImage*)getIDCardImage;
- (UIImage*)getHeadImage;
- (NSArray *)doBCR;

- (void)setProgressCallbackDelegate:(id<BCRProgressCallBackDelegate>)delegate;
- (void)progressCancel;

-(int)doBcrRecognizeVedioWith:(int)width andHeight:(int)height andRect:(BRect)pRect andChannelNumberStr:(NSString *)channelNumberStr;
-(void)setBcrResultCallbackDelegate:(id)delegate;

@end

@protocol BCRProgressCallBackDelegate <NSObject>

- (void)progressCallbackWithValue:(NSInteger)value;
- (void)progressStop;

@end

