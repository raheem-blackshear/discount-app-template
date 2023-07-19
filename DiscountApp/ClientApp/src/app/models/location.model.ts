export class Location {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, name?: string, value?: string) {

        this.locationId = id;
        this.locationName = name;
        this.locationValue = value;      

    }

    public locationId: string;
    public locationName: string;
    public locationValue: string; 
    public IsActive: boolean;

}
