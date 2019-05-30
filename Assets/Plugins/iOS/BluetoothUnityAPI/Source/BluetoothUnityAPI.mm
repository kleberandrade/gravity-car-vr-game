//
//  Wrapper.m
//  BluetoothUnityAPI
//
//  Created by MAC on 5/2/19.
//  Copyright © 2019 Tony Abou Zaidan. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>
#import <BluetoothUnityAPI/BluetoothUnityAPI-Swift.h>

typedef void (*BluetoothVoidEvent)();

typedef void (*BluetoothStringEvent) (const char*);

@interface BluetoothEventWrapper : NSObject <BluetoothEventListener>

@property (nonatomic, readwrite) BluetoothVoidEvent _onConnected;
@property (nonatomic, readwrite) BluetoothVoidEvent _onConnectionFailed;
@property (nonatomic, readwrite) BluetoothStringEvent _onDataReceived;
@property (nonatomic, readwrite) BluetoothVoidEvent _onScanEnded;
@end

@interface ObjCBluetoothHelper : NSObject

@property (nonatomic, readwrite) BluetoothHelper * helper;
@property (nonatomic, readwrite) BluetoothEventWrapper * wrapper;

@end

@implementation ObjCBluetoothHelper

static ObjCBluetoothHelper * instance;

+(ObjCBluetoothHelper*) getInstance
{
    return instance;
}

-(void)initHelper:(NSString*) deviceName
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        NSError *error = nil;
        self.helper = [[BluetoothHelper alloc] initWithDeviceName:deviceName error:&error];
        self.wrapper = [[BluetoothEventWrapper alloc] init];
        [self.helper setOnDataReceivedListener:self.wrapper];
        instance = self;
    });
    
}

-(BluetoothHelper *) getHelper
{
    return self.helper;
}

-(BluetoothEventWrapper *) getWrapper
{
    return self.wrapper;
}

@end

@implementation BluetoothEventWrapper

@synthesize _onConnected, _onConnectionFailed, _onDataReceived, _onScanEnded;

- (void)OnConnected {
    _onConnected();
}

- (void)OnConnectionFailed {
    _onConnectionFailed();
}

- (void)OnDataReceived:(NSString *)data {
    const char * str = [data cStringUsingEncoding:NSUTF8StringEncoding];
    self._onDataReceived(str);
}

- (void)OnScanEnded
{
    _onScanEnded();
}

- (void)setOnConnected:(BluetoothVoidEvent)onConnected
{
    self._onConnected = onConnected;
}

- (void) setOnConnectionFailed:(BluetoothVoidEvent)onConnectionFailed
{
    self._onConnectionFailed = onConnectionFailed;
}

-(void) setOnDataReceived:(BluetoothStringEvent)onDataReceived
{
    self._onDataReceived = onDataReceived;
}

- (void) setOnScanEnded:(BluetoothVoidEvent)onScanEnded
{
    self._onScanEnded = onScanEnded;
}
@end


extern "C" {
    
    void __GetInstance(const char * deviceName, BluetoothVoidEvent OnConnected, BluetoothVoidEvent OnConnectionFailed, /*void (*OnDataReceived)(const char*),*/ BluetoothVoidEvent OnScanEnded, BluetoothStringEvent OnDataReceived)
    {
        ObjCBluetoothHelper * helper = [[ObjCBluetoothHelper alloc] init];
        NSString * _deviceName = [NSString stringWithUTF8String:deviceName];
        [helper initHelper:_deviceName];
        [[helper getWrapper] setOnConnected:OnConnected];
        [[helper getWrapper] setOnConnectionFailed:OnConnectionFailed];
        [[helper getWrapper] setOnScanEnded:OnScanEnded];
        [[helper getWrapper] setOnDataReceived:OnDataReceived];
    }
    
    bool __ScanNearbyDevices()
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        return [helper ScanNearbyDevices];
    }
    
    void __startListening()
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        NSError * error = nil;
        [helper startListeningAndReturnError:&error];
    }
    
    void __Connect()
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        NSError * error = nil;
        [helper ConnectAndReturnError:&error];
    }
    
    bool __isDevicePaired()
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        return [helper isDevicePaired];
    }
    
    bool __isConnected()
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        return [helper isConnected];
    }
    
    void __setDeviceName(const char * deviceName)
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        [helper setDeviceName:[NSString stringWithUTF8String:deviceName]];
    }
    
    void __setDeviceAddress(const char * deviceAddress)
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        NSString * _deviceAddress = [NSString stringWithUTF8String:deviceAddress];
        [helper setDeviceAddress:_deviceAddress];
    }
    
    void __sendData(const char * data)
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        NSString * _data = [NSString stringWithUTF8String:data];
        NSError * error = nil;
        [helper sendData:_data error:&error];
    }
    
    void __Disconnect()
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        [helper Disconnect];
    }
    
    void __setTerminatorBasedStream(const char * separator)
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        NSString * _separator = [NSString stringWithUTF8String:separator];
        [helper setTerminatorBasedStream:_separator];
    }
    
    void __setLengthBasedStream()
    {
        BluetoothHelper * helper = [[ObjCBluetoothHelper getInstance] getHelper];
        [helper setLengthBasedStream];
    }
    
    int __getNearbyDevicesArrayLength()
    {
        return (int)[[[[ObjCBluetoothHelper getInstance] getHelper] getNearbyDevices] count];
    }
    
    const char * __getNearbyDeviceNameAt(int index)
    {
        NSArray<BluetoothDevice * > * nb = [[[ObjCBluetoothHelper getInstance] getHelper] getNearbyDevices];
        return [[[nb objectAtIndex:index] getDeviceName] cStringUsingEncoding:NSASCIIStringEncoding];
    }
    
    const char * __getNearbyDeviceAddressAt(int index)
    {
        NSArray<BluetoothDevice * > * nb = [[[ObjCBluetoothHelper getInstance] getHelper] getNearbyDevices];
        return [[[nb objectAtIndex:index] getDeviceAddress] cStringUsingEncoding:NSASCIIStringEncoding];
    }
    
    int __getLastError()
    {
        return (int) [[[ObjCBluetoothHelper getInstance] getHelper] getLastError];
    }
}