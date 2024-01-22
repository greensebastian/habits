#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from 'aws-cdk-lib';
import { InfraStack } from '../lib/infraStack';
import { GitHubOidcStack } from '../lib/githubOidcStack';

const app = new cdk.App();
const env = { account: '538729212824', region: 'eu-west-1' }

var gitHubStack = new GitHubOidcStack(app, 'GitHubOidcStack', {
  repositoryConfig: [
    {
      owner: 'greensebastian',
      repoName: 'habits',
    }
  ],
  env
})

new InfraStack(app, 'InfraStack', {
  githubRole: gitHubStack.gitHubPrincipal,
  env
});