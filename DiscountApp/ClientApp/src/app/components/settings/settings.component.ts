// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/switchMap';

import { fadeInOut } from '../../services/animations';
import { BootstrapTabDirective } from "../../directives/bootstrap-tab.directive";
import { AccountService } from "../../services/account.service";
import { Permission } from '../../models/permission.model';


@Component({
  selector: 'settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  animations: [fadeInOut]
})
export class SettingsComponent implements OnInit, OnDestroy {

  isProfileActivated = true;
  isPreferencesActivated = false;
  isUsersActivated = false;
  isRolesActivated = false;
  isTypeConsumesActivated = false;
  isAgesActivated = false;
  isSchoolsActivated = false;
  isNationalitiesActivated = false;



  fragmentSubscription: any;

    readonly profileTab = "profile";
    readonly preferencesTab = "preferences";
    readonly usersTab = "users";
    readonly rolesTab = "roles";
    readonly typeConsumesTab = "typeConsumes";
    readonly agesTab = "ages";
    readonly nationalitiesTab = "nationalities";
    readonly schoolsTab = "schools";


  @ViewChild("tab")
  tab: BootstrapTabDirective;


  constructor(private route: ActivatedRoute, private accountService: AccountService) {
  }


  ngOnInit() {
    this.fragmentSubscription = this.route.fragment.subscribe(anchor => this.showContent(anchor));
  }


  ngOnDestroy() {
    this.fragmentSubscription.unsubscribe();
  }

  showContent(anchor: string) {
    if ((this.isFragmentEquals(anchor, this.usersTab) && !this.canViewUsers) ||
      (this.isFragmentEquals(anchor, this.rolesTab) && !this.canViewRoles))
      return;

    this.tab.show(`#${anchor || this.profileTab}Tab`);
  }


  isFragmentEquals(fragment1: string, fragment2: string) {

    if (fragment1 == null)
      fragment1 = "";

    if (fragment2 == null)
      fragment2 = "";

    return fragment1.toLowerCase() == fragment2.toLowerCase();
  }


  onShowTab(event) {
    let activeTab = event.target.hash.split("#", 2).pop();

      this.isProfileActivated = activeTab == this.profileTab;
      this.isPreferencesActivated = activeTab == this.preferencesTab;
      this.isUsersActivated = activeTab == this.usersTab;
      this.isRolesActivated = activeTab == this.rolesTab;
      this.isTypeConsumesActivated = activeTab == this.typeConsumesTab;
      this.isAgesActivated = activeTab == this.agesTab;
      this.isNationalitiesActivated = activeTab == this.nationalitiesTab;
      this.isSchoolsActivated = activeTab == this.schoolsTab;
  }


  get canViewUsers() {
    return this.accountService.userHasPermission(Permission.viewUsersPermission);
  }

  get canViewRoles() {
    return this.accountService.userHasPermission(Permission.viewRolesPermission);
  }
}
