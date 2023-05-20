export class UserViewModel  {
    id!:number
    username! : string
    email! : string
    profileImage? : string
    designation?:string
    subtitleUpload?:number
    subtitleDownload?:number
    funFact?:string
    prefferedLanguages?:any[]
    registrationDate?:Date
    isBanned!:boolean
}