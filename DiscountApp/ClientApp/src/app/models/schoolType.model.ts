
export class SchoolType {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, name?: string) {

        this.schoolTypeId = id;
        this.schoolTypeName = name;        

    }

    public schoolTypeId: string;
    public schoolTypeName: string; 

}
