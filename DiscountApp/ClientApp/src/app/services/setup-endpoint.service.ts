import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';


@Injectable()
export class SetupEndpointService extends EndpointFactory  {

    private readonly _typeConsumesUrl: string = "/api/typeConsume/typeConsumes";
    private readonly _typeConsumeBytypeConsumeNameUrl: string = "/api/typeConsume/typeConsumes/typeConsumename";
    private readonly _currenttypeConsumeUrl: string = "/api/typeConsume/typeConsumes/me";

    private readonly _schoolTypeUrl: string = "/api/SchoolType/schoolType";
    private readonly _schoolUrl: string = "/api/school/school";
    private readonly _schoolByschoolNameUrl: string = "/api/school/school/schoolname";
    private readonly _currentschoolUrl: string = "/api/school/school/me";


    private readonly _ageUrl: string = "/api/age/age";
    private readonly _ageByageNameUrl: string = "/api/age/age/agename";
    private readonly _currentageUrl: string = "/api/age/age/me";

    private readonly _helpUrl: string = "/api/help/help";
    private readonly _helpByhelpNameUrl: string = "/api/help/help/helpname";
    private readonly _currenthelpUrl: string = "/api/help/help/me";

    private readonly _locationUrl: string = "/api/location/location";
    private readonly _locationBylocationNameUrl: string = "/api/location/location/locationname";
    private readonly _currentlocationUrl: string = "/api/location/location/me";


    private readonly _nationalityUrl: string = "/api/nationality/nationality";
    private readonly _nationalityBynationalityNameUrl: string = "/api/nationality/nationality/agename";
    private readonly _currentnationalityUrl: string = "/api/nationality/nationality/me";


    get typeConsumesUrl() { return this.configurations.baseUrl + this._typeConsumesUrl; }
    get typeConsumeByTypeConsumeNameUrl() { return this.configurations.baseUrl + this._typeConsumeBytypeConsumeNameUrl; }
    get currentTypeConsumeUrl() { return this.configurations.baseUrl + this._typeConsumesUrl; }

    
    get ageUrl() { return this.configurations.baseUrl + this._ageUrl; }
    get ageByAgeNameUrl() { return this.configurations.baseUrl + this._ageByageNameUrl; }
    get currentAgeUrl() { return this.configurations.baseUrl + this._ageUrl; }

    get helpUrl() { return this.configurations.baseUrl + this._helpUrl; }
    get helpByHelpNameUrl() { return this.configurations.baseUrl + this._helpByhelpNameUrl; }
    get currentHelpUrl() { return this.configurations.baseUrl + this._helpUrl; }


    get locationUrl() { return this.configurations.baseUrl + this._locationUrl; }
    get locationByLocationNameUrl() { return this.configurations.baseUrl + this._locationBylocationNameUrl; }
    get currentLocationUrl() { return this.configurations.baseUrl + this._locationUrl; }

    get nationalityUrl() { return this.configurations.baseUrl + this._nationalityUrl; }
    get nationalityByNationalityNameUrl() { return this.configurations.baseUrl + this._nationalityBynationalityNameUrl; }
    get currentNationalityUrl() { return this.configurations.baseUrl + this._nationalityUrl; }

    get schoolUrl() { return this.configurations.baseUrl + this._schoolUrl; }
    get schoolTypeUrl() { return this.configurations.baseUrl + this._schoolTypeUrl; }
    get schoolBySchoolNameUrl() { return this.configurations.baseUrl + this._schoolByschoolNameUrl; }
    get currentSchoolUrl() { return this.configurations.baseUrl + this._schoolUrl; }



    getTypeConsumeEndpoint<T>(typeConsumesId?: string): Observable<T> {
        let endpointUrl = typeConsumesId ? `${this.typeConsumesUrl}/${typeConsumesId}` : this.currentTypeConsumeUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getTypeConsumeEndpoint(typeConsumesId));
            });
    }

    getTypeConsumesEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
        let endpointUrl = page && pageSize ? `${this.typeConsumesUrl}/${page}/${pageSize}` : this.typeConsumesUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getTypeConsumesEndpoint(page, pageSize));
            });
    }

    getDeleteTypeConsumeEndpoint<T>(typeConsumesId: string): Observable<T> {
        let endpointUrl = `${this.typeConsumesUrl}/${typeConsumesId}`;

        return this.http.delete<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getDeleteTypeConsumeEndpoint(typeConsumesId));
            });
    }

    getTypeConsumeByTypeConsumeNameEndpoint<T>(typeConsume: string): Observable<T> {
        let endpointUrl = `${this.typeConsumeByTypeConsumeNameUrl}/${typeConsume}`;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getTypeConsumeByTypeConsumeNameEndpoint(typeConsume));
            });
    }

    getUpdateTypeConsumeEndpoint<T>(typeConsumeObject: any, typeConsumeuserId?: string): Observable<T> {
        let endpointUrl = `${this.typeConsumesUrl}/${typeConsumeuserId}`;

        return this.http.put<T>(endpointUrl, JSON.stringify(typeConsumeObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getUpdateTypeConsumeEndpoint(typeConsumeObject, typeConsumeuserId));
            });
    }

    getNewTypeConsumeEndpoint<T>(typeConsumeObject: any): Observable<T> {

        return this.http.post<T>(this.typeConsumesUrl, JSON.stringify(typeConsumeObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNewTypeConsumeEndpoint(typeConsumeObject));
            });
    }



    getAgeEndpoint<T>(ageId?: string): Observable<T> {
        let endpointUrl = ageId ? `${this.ageUrl}/${ageId}` : this.currentAgeUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getAgeEndpoint(ageId));
            });
    }

    getAgesEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
        let endpointUrl = page && pageSize ? `${this.ageUrl}/${page}/${pageSize}` : this.ageUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getAgesEndpoint(page, pageSize));
            });
    }

    getDeleteAgeEndpoint<T>(ageId: string): Observable<T> {
        let endpointUrl = `${this.ageUrl}/${ageId}`;

        return this.http.delete<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getDeleteAgeEndpoint(ageId));
            });
    }




    getHelpEndpoint<T>(helpId?: string): Observable<T> {
        let endpointUrl = helpId ? `${this.helpUrl}/${helpId}` : this.currentAgeUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getHelpEndpoint(helpId));
            });
    }


    getLocationEndpoint<T>(locationId?: string): Observable<T> {
        let endpointUrl = locationId ? `${this.locationUrl}/${locationId}` : this.currentLocationUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getLocationEndpoint(locationId));
            });
    }

    getSchoolEndpoint<T>(schoolId?: string): Observable<T> {
        let endpointUrl = schoolId ? `${this.schoolUrl}/${schoolId}` : this.currentSchoolUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getSchoolEndpoint(schoolId));
            });
    }

    getSchoolTypeEndpoint<T>(): Observable<T> {
        let endpointUrl =  `${this.schoolTypeUrl}` ;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getSchoolTypeEndpoint());
            });
    }

     


    getNationalityEndpoint<T>(nationalityId?: string): Observable<T> {
        let endpointUrl = nationalityId ? `${this.nationalityUrl}/${nationalityId}` : this.currentNationalityUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNationalityEndpoint(nationalityId));
            });
    }



   


   

    getHelpsEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
        let endpointUrl = page && pageSize ? `${this.helpUrl}/${page}/${pageSize}` : this.helpUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getHelpsEndpoint(page, pageSize));
            });
    }

    getLocationsEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
        let endpointUrl = page && pageSize ? `${this.locationUrl}/${page}/${pageSize}` : this.locationUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getLocationsEndpoint(page, pageSize));
            });
    }

    getSchoolsEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
        let endpointUrl = page && pageSize ? `${this.schoolUrl}/${page}/${pageSize}` : this.schoolUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getSchoolsEndpoint(page, pageSize));
            });
    }

    getNationalitiesEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
        let endpointUrl = page && pageSize ? `${this.nationalityUrl}/${page}/${pageSize}` : this.nationalityUrl;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNationalitiesEndpoint(page, pageSize));
            });
    }


   

   


    getDeleteHelpEndpoint<T>(helpId: string): Observable<T> {
        let endpointUrl = `${this.helpUrl}/${helpId}`;

        return this.http.delete<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getDeleteHelpEndpoint(helpId));
            });
    }


    getDeleteLocationEndpoint<T>(locationId: string): Observable<T> {
        let endpointUrl = `${this.locationUrl}/${locationId}`;

        return this.http.delete<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getDeleteLocationEndpoint(locationId));
            });
    }

    getDeleteSchoolEndpoint<T>(schoolId: string): Observable<T> {
        let endpointUrl = `${this.schoolUrl}/${schoolId}`;

        return this.http.delete<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getDeleteSchoolEndpoint(schoolId));
            });
    }

    getDeleteNationalityEndpoint<T>(nationalityId: string): Observable<T> {
        let endpointUrl = `${this.nationalityUrl}/${nationalityId}`;

        return this.http.delete<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getDeleteNationalityEndpoint(nationalityId));
            });
    }

    
    getAgeByAgeNameEndpoint<T>(age: string): Observable<T> {
        let endpointUrl = `${this.ageByAgeNameUrl}/${age}`;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getAgeByAgeNameEndpoint(age));
            });
    }

    getHelpByHelpNameEndpoint<T>(help: string): Observable<T> {
        let endpointUrl = `${this.helpByHelpNameUrl}/${help}`;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getHelpByHelpNameEndpoint(help));
            });
    }


    getLocationByLocationNameEndpoint<T>(location: string): Observable<T> {
        let endpointUrl = `${this.locationByLocationNameUrl}/${location}`;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getLocationByLocationNameEndpoint(location));
            });
    }

    getSchoolBySchoolNameEndpoint<T>(school: string): Observable<T> {
        let endpointUrl = `${this.schoolBySchoolNameUrl}/${school}`;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getSchoolBySchoolNameEndpoint(school));
            });
    }

    getNationalityByNationalityNameEndpoint<T>(nationality: string): Observable<T> {
        let endpointUrl = `${this.nationalityByNationalityNameUrl}/${nationality}`;

        return this.http.get<T>(endpointUrl, this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNationalityByNationalityNameEndpoint(nationality));
            });
    }
    
   

    getNewAgeEndpoint<T>(ageObject: any): Observable<T> {

        return this.http.post<T>(this.ageUrl, JSON.stringify(ageObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNewAgeEndpoint(ageObject));
            });
    }

    getNewHelpEndpoint<T>(ageObject: any): Observable<T> {

        return this.http.post<T>(this.helpUrl, JSON.stringify(ageObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNewHelpEndpoint(ageObject));
            });
    }


    getNewLocationEndpoint<T>(locationObject: any): Observable<T> {

        return this.http.post<T>(this.locationUrl, JSON.stringify(locationObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNewLocationEndpoint(locationObject));
            });
    }

    getNewSchoolEndpoint<T>(schoolObject: any): Observable<T> {

        return this.http.post<T>(this.schoolUrl, JSON.stringify(schoolObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNewSchoolEndpoint(schoolObject));
            });
    }


    getNewNationalityEndpoint<T>(nationalityObject: any): Observable<T> {

        return this.http.post<T>(this.nationalityUrl, JSON.stringify(nationalityObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getNewNationalityEndpoint(nationalityObject));
            });
    }



    

    getUpdateSchoolEndpoint<T>(schoolObject: any, schooluserId?: string): Observable<T> {
        let endpointUrl = `${this.schoolUrl}/${schooluserId}`;

        return this.http.put<T>(endpointUrl, JSON.stringify(schoolObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getUpdateSchoolEndpoint(schoolObject, schooluserId));
            });
    }

    getUpdateLocationEndpoint<T>(locationObject: any, locationuserId?: string): Observable<T> {
        let endpointUrl = `${this.locationUrl}/${locationuserId}`;

        return this.http.put<T>(endpointUrl, JSON.stringify(locationObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getUpdateLocationEndpoint(locationObject, locationuserId));
            });
    }


    getUpdateAgeEndpoint<T>(ageObject: any, ageuserId?: string): Observable<T> {
        let endpointUrl = `${this.ageUrl}/${ageuserId}`;

        return this.http.put<T>(endpointUrl, JSON.stringify(ageObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getUpdateAgeEndpoint(ageObject, ageuserId));
            });
    }

    getUpdateHelpEndpoint<T>(ageObject: any, helpuserId?: string): Observable<T> {
        let endpointUrl = `${this.helpUrl}/${helpuserId}`;

        return this.http.put<T>(endpointUrl, JSON.stringify(ageObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getUpdateHelpEndpoint(ageObject, helpuserId));
            });
    }

    getUpdateNationalityEndpoint<T>(nationalityObject: any, nationalityuserId?: string): Observable<T> {
        let endpointUrl = `${this.nationalityUrl}/${nationalityuserId}`;

        return this.http.put<T>(endpointUrl, JSON.stringify(nationalityObject), this.getRequestHeaders())
            .catch(error => {
                return this.handleError(error, () => this.getUpdateAgeEndpoint(nationalityObject, nationalityuserId));
            });
    }


}
