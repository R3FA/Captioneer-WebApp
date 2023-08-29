import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PageLoginComponent } from './Components/Page-LogIn/page-login/page-login.component';
import { SignupPageComponent } from './Components/signup-page/signup-page.component';
import { ProfilePageComponent } from './Components/Page-ProfileUI/profile-page/profile-page.component';
import { PageCommentComponent } from './Components/Page-Comments/page-comment/page-comment.component';
import { HomepageComponent } from './Components/homepage/homepage.component';
import { MovieInfoComponent } from './Components/movie-info/movie-info.component';
import { PageDirectMessagesComponent } from './Components/page-direct-messages/page-direct-messages.component';
import { PageNotFoundErrorComponent } from './Components/page-not-found-error/page-not-found-error.component';
import { FollowFriendsComponent } from './Components/Page-ProfileUI/profile-page/follow-friends/follow-friends.component';
import { UserSearchComponent } from './Components/Page-ProfileUI/profile-page/user-search/user-search.component';
import { VerifyComponent } from './Components/verify/verify.component';

const appRoute: Routes = [
    { path: '', redirectTo: 'Home', pathMatch: 'full' },
    { path: 'Home', component: HomepageComponent },
    { path: 'Movie', component: MovieInfoComponent },
    { path: 'Movie/Comments', component: PageCommentComponent },
    { path: 'Signup', component: SignupPageComponent },
    { path: 'Signin', component: PageLoginComponent },
    { path: 'Verify', component: VerifyComponent },
    {
        path: 'Profile/:id', component: ProfilePageComponent,
        children: [
            { path: 'addFriends', component: FollowFriendsComponent },
            { path: 'searchUser', component: UserSearchComponent }
        ]
    },
    { path: 'Profile/:id/directMessages', component: PageDirectMessagesComponent },
    { path: '**', component: PageNotFoundErrorComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(appRoute)],
    exports: [RouterModule],
    providers: []
})

export class AppRoutingModule {

}