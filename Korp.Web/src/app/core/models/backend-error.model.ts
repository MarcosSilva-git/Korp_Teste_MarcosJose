export interface BackendError {
    title: string
    status: number
    detail: string
    errors?: any[]
}