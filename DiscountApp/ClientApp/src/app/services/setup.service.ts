import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from "@angular/router";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/observable/forkJoin';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { SetupEndpointService } from './setup-endpoint.service';
import { TypeConsume } from '../../app/models/typeConsume.model';
import { Age } from '../models/age.model';
import { Help } from '../models/help.model';
import { Nationality } from '../models/nationality.model';
import { School } from '../models/school.model';
import { SchoolType } from '../models/schoolType.model';
import { Location } from '../models/location.model';



@Injectable()
export class SetupService {

    constructor(private router: Router, private http: HttpClient, private setupEndpoint: SetupEndpointService) {

    }

    getTypeConsume(typeConsumeId?: string) {
        return this.setupEndpoint.getTypeConsumeEndpoint<TypeConsume>(typeConsumeId);
    }

    getTypeConsumes(page?: number, pageSize?: number) {

        return this.setupEndpoint.getTypeConsumesEndpoint<TypeConsume[]>(page, pageSize);
    }

    deleteTypeConsume(typeConsumeOrTypeConsumeId: string | TypeConsume): Observable<TypeConsume> {

        if (typeof typeConsumeOrTypeConsumeId === 'string' || typeConsumeOrTypeConsumeId instanceof String) {
            return this.setupEndpoint.getDeleteTypeConsumeEndpoint<TypeConsume>(<string>typeConsumeOrTypeConsumeId);
        }
        else {

            if (typeConsumeOrTypeConsumeId.typeConsumeId) {
                return this.deleteTypeConsume(typeConsumeOrTypeConsumeId.typeConsumeId);
            }
            else {
                return this.setupEndpoint.getTypeConsumeByTypeConsumeNameEndpoint<TypeConsume>(typeConsumeOrTypeConsumeId.typeConsumeName)
                    .mergeMap(type => this.deleteTypeConsume(type.typeConsumeId));
            }
        }
    }

    updateTypeConsume(typeConsume: TypeConsume) {
        if (typeConsume.typeConsumeId) {
            return this.setupEndpoint.getUpdateTypeConsumeEndpoint(typeConsume, typeConsume.typeConsumeId);
        }
        else {
            return this.setupEndpoint.getTypeConsumeByTypeConsumeNameEndpoint<TypeConsume>(typeConsume.typeConsumeName)
                .mergeMap(foundUser => {
                    typeConsume.typeConsumeId = foundUser.typeConsumeId;
                    return this.setupEndpoint.getUpdateTypeConsumeEndpoint(typeConsume, typeConsume.typeConsumeId)
                });
        }
    }

    newTypeConsume(typeConsume: TypeConsume) {
        return this.setupEndpoint.getNewTypeConsumeEndpoint<TypeConsume>(typeConsume);
    }


    getHelp(helpId?: string) {
        return this.setupEndpoint.getHelpEndpoint<Help>(helpId);
    }

    getHelps(page?: number, pageSize?: number) {

        return this.setupEndpoint.getHelpsEndpoint<Help[]>(page, pageSize);
    }

    deleteHelp(helpOrHelpId: string | Help): Observable<Help> {

        if (typeof helpOrHelpId === 'string' || helpOrHelpId instanceof String) {
            return this.setupEndpoint.getDeleteHelpEndpoint<Help>(<string>helpOrHelpId);
        }
        else {

            if (helpOrHelpId.helpId) {
                return this.deleteHelp(helpOrHelpId.helpId);
            }
            else {
                return this.setupEndpoint.getHelpByHelpNameEndpoint<Help>(helpOrHelpId.helpName)
                    .mergeMap(type => this.deleteHelp(type.helpId));
            }
        }
    }

    updateHelp(help: Help) {
        if (help.helpId) {
            return this.setupEndpoint.getUpdateHelpEndpoint(help, help.helpId);
        }
        else {
            return this.setupEndpoint.getHelpByHelpNameEndpoint<Help>(help.helpName)
                .mergeMap(foundUser => {
                    help.helpId = foundUser.helpId;
                    return this.setupEndpoint.getUpdateAgeEndpoint(help, help.helpId)
                });
        }
    }

    newHelp(help: Help) {
        return this.setupEndpoint.getNewHelpEndpoint<Help>(help);
    }



    getAge(ageId?: string) {
        return this.setupEndpoint.getAgeEndpoint<Age>(ageId);
    }

    getAges(page?: number, pageSize?: number) {

        return this.setupEndpoint.getAgesEndpoint<Age[]>(page, pageSize);
    }

    deleteAge(ageOrAgeId: string | Age): Observable<Age> {

        if (typeof ageOrAgeId === 'string' || ageOrAgeId instanceof String) {
            return this.setupEndpoint.getDeleteAgeEndpoint<Age>(<string>ageOrAgeId);
        }
        else {

            if (ageOrAgeId.ageId) {
                return this.deleteAge(ageOrAgeId.ageId);
            }
            else {
                return this.setupEndpoint.getAgeByAgeNameEndpoint<Age>(ageOrAgeId.ageName)
                    .mergeMap(type => this.deleteAge(type.ageId));
            }
        }
    }

    updateAge(age: Age) {
        if (age.ageId) {
            return this.setupEndpoint.getUpdateAgeEndpoint(age, age.ageId);
        }
        else {
            return this.setupEndpoint.getAgeByAgeNameEndpoint<Age>(age.ageName)
                .mergeMap(foundUser => {
                    age.ageId = foundUser.ageId;
                    return this.setupEndpoint.getUpdateAgeEndpoint(age, age.ageId)
                });
        }
    }

    newAge(age: Age) {
        return this.setupEndpoint.getNewAgeEndpoint<Age>(age);
    }



    getLocation(locationId?: string) {
        return this.setupEndpoint.getLocationEndpoint<Location>(locationId);
    }

    getLocations(page?: number, pageSize?: number) {

        return this.setupEndpoint.getLocationsEndpoint<Location[]>(page, pageSize);
    }

    deleteLocation(locationOrLocationId: string | Location): Observable<Location> {

        if (typeof locationOrLocationId === 'string' || locationOrLocationId instanceof String) {
            return this.setupEndpoint.getDeleteLocationEndpoint<Location>(<string>locationOrLocationId);
        }
        else {

            if (locationOrLocationId.locationId) {
                return this.deleteLocation(locationOrLocationId.locationId);
            }
            else {
                return this.setupEndpoint.getLocationByLocationNameEndpoint<Location>(locationOrLocationId.locationName)
                    .mergeMap(type => this.deleteLocation(type.locationId));
            }
        }
    }

    updateLocation(location: Location) {
        if (location.locationId) {
            return this.setupEndpoint.getUpdateLocationEndpoint(location, location.locationId);
        }
        else {
            return this.setupEndpoint.getLocationByLocationNameEndpoint<Location>(location.locationName)
                .mergeMap(foundUser => {
                    location.locationId = foundUser.locationId;
                    return this.setupEndpoint.getUpdateLocationEndpoint(location, location.locationId)
                });
        }
    }

    newLocation(location: Location) {
        return this.setupEndpoint.getNewLocationEndpoint<Location>(location);
    }



    getSchool(schoolId?: string) {
        return this.setupEndpoint.getSchoolEndpoint<School>(schoolId);
    }

    getSchoolType() {
        return this.setupEndpoint.getSchoolTypeEndpoint<SchoolType>();
    }

    getSchools(page?: number, pageSize?: number) {

        return this.setupEndpoint.getSchoolsEndpoint<School[]>(page, pageSize);
    }

    deleteSchool(schoolOrSchoolId: string | School): Observable<School> {

        if (typeof schoolOrSchoolId === 'string' || schoolOrSchoolId instanceof String) {
            return this.setupEndpoint.getDeleteSchoolEndpoint<School>(<string>schoolOrSchoolId);
        }
        else {

            if (schoolOrSchoolId.schoolId) {
                return this.deleteSchool(schoolOrSchoolId.schoolId);
            }
            else {
                return this.setupEndpoint.getSchoolBySchoolNameEndpoint<School>(schoolOrSchoolId.schoolName)
                    .mergeMap(type => this.deleteSchool(type.schoolId));
            }
        }
    }

    updateSchool(school: School) {
        if (school.schoolId) {
            return this.setupEndpoint.getUpdateSchoolEndpoint(school, school.schoolId);
        }
        else {
            return this.setupEndpoint.getSchoolBySchoolNameEndpoint<School>(school.schoolName)
                .mergeMap(foundUser => {
                    school.schoolId = foundUser.schoolId;
                    return this.setupEndpoint.getUpdateSchoolEndpoint(school, school.schoolId)
                });
        }
    }

    newSchool(school: School) {
        return this.setupEndpoint.getNewSchoolEndpoint<School>(school);
    }


    getNationality(nationalityId?: string) {
        return this.setupEndpoint.getNationalityEndpoint<Nationality>(nationalityId);
    }


    getNationalities(page?: number, pageSize?: number) {

        return this.setupEndpoint.getNationalitiesEndpoint<Nationality[]>(page, pageSize);
    }


    deleteNationality(nationalityOrNationalityId: string | Nationality): Observable<Nationality> {

        if (typeof nationalityOrNationalityId === 'string' || nationalityOrNationalityId instanceof String) {
            return this.setupEndpoint.getDeleteNationalityEndpoint<Nationality>(<string>nationalityOrNationalityId);
        }
        else {

            if (nationalityOrNationalityId.nationalityId) {
                return this.deleteNationality(nationalityOrNationalityId.nationalityId);
            }
            else {
                return this.setupEndpoint.getNationalityByNationalityNameEndpoint<TypeConsume>(nationalityOrNationalityId.nationalityName)
                    .mergeMap(type => this.deleteNationality(type.typeConsumeId));
            }
        }
    }


    updateNationality(nationality: Nationality) {
        if (nationality.nationalityId) {
            return this.setupEndpoint.getUpdateNationalityEndpoint(nationality, nationality.nationalityId);
        }
        else {
            return this.setupEndpoint.getNationalityByNationalityNameEndpoint<Nationality>(nationality.nationalityName)
                .mergeMap(foundUser => {
                    nationality.nationalityId = foundUser.nationalityId;
                    return this.setupEndpoint.getUpdateNationalityEndpoint(nationality, nationality.nationalityId)
                });
        }
    }


    newNationality(nationality: Nationality) {
        return this.setupEndpoint.getNewNationalityEndpoint<Nationality>(nationality);
    }

}
