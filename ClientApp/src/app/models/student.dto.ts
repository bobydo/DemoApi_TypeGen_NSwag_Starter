export interface StudentDto {
    studentId?: number;
    studentNo?: string;
    name?: string;
    active?: boolean;
    //should be lazy loading or on-demand loading
    //load addresses for ALL students immediately, which is slower but requires fewer API calls.
    //addresses?: AddressDto[];  Always included
}
