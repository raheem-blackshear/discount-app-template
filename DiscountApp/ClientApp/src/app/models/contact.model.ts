// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

export class Contact {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(name?: string, email?: string, mobile?: string, subject?: string, message?: string, phoneNumber?: string) {

        this.name = name;
        this.email = email;
        this.mobile = mobile;
        this.subject = subject;
        this.message = message;
        this.phoneNumber = phoneNumber;    
    }

       
    public name: string;
    public email: string;
    public mobile: string;
    public subject: string;
    public message: string;
    public phoneNumber: string;
  
}
