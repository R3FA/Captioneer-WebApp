import { UserViewModel } from "./user-viewmodel"

export class UserResponse {
    currentPage: number = 1
    pages!: number
    users?: UserViewModel[] = []
}