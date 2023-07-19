
export class School {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, name?: string, value?: string, type?:number) {

        this.schoolId = id;
        this.schoolName = name;
        this.schoolValue = value;
        this.schoolTypeId = type;      

    }

    public schoolId: string;
    public schoolName: string;
    public schoolValue: string;
    public schoolTypeId: number; 
    public isSchool: boolean;

}
