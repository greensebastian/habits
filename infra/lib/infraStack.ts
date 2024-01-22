import * as cdk from 'aws-cdk-lib';
import { IGrantable } from 'aws-cdk-lib/aws-iam';
import { Construct } from 'constructs';
// import * as sqs from 'aws-cdk-lib/aws-sqs';

export interface InfraStackProps extends cdk.StackProps {
  githubRole: IGrantable;
}

export class InfraStack extends cdk.Stack {
  db: cdk.aws_rds.DatabaseCluster;

  constructor(scope: Construct, id: string, props: InfraStackProps) {
    super(scope, id, props);

    const repo = new cdk.aws_ecr.Repository(this, 'Repository', {
      repositoryName: 'habits',
      removalPolicy: cdk.RemovalPolicy.DESTROY,
      autoDeleteImages: true,
    });

    repo.grantPullPush(props.githubRole);

    const defaultVpc = cdk.aws_ec2.Vpc.fromLookup(this, 'DefaultVPC', { isDefault: true });

    const databaseSecurityGroup = new cdk.aws_ec2.SecurityGroup(this, 'DatabaseSecurityGroup', {
      vpc: defaultVpc,
      allowAllOutbound: false
    });
    databaseSecurityGroup.addIngressRule(cdk.aws_ec2.Peer.anyIpv4(), cdk.aws_ec2.Port.tcp(5432), 'Inbound PostgreSql');

    this.db = new cdk.aws_rds.DatabaseCluster(this, 'Database', {
      engine: cdk.aws_rds.DatabaseClusterEngine.auroraPostgres({ version: cdk.aws_rds.AuroraPostgresEngineVersion.VER_15_3 }),
      credentials: cdk.aws_rds.Credentials.fromGeneratedSecret('postgres'),
      vpc: defaultVpc,
      writer: cdk.aws_rds.ClusterInstance.serverlessV2('WriterClusterInstance', {
        publiclyAccessible: true,
      }),
      readers: [
        cdk.aws_rds.ClusterInstance.serverlessV2('ReaderClusterInstance', {
          publiclyAccessible: true,
        })
      ],
      vpcSubnets: {
        subnetType: cdk.aws_ec2.SubnetType.PUBLIC,
      },
      serverlessV2MinCapacity: 0.5,
      serverlessV2MaxCapacity: 1.0,
      cloudwatchLogsRetention: cdk.aws_logs.RetentionDays.ONE_MONTH,
      securityGroups: [databaseSecurityGroup]
    });
  }
}
