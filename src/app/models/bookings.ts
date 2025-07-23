export interface bookings{
    bookingId:number;
    userId:number;
    floorId:number;
    buildingId:number;
    roomsId:number;
    roomsType:string;
    startTime:Date;
    endTime:Date;
    isExpired:boolean;
}