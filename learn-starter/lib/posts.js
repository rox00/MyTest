import fs from 'fs'
import path from 'path'
import matter from 'gray-matter'
import { useState } from 'react';

// import * as parseArgs from 'minimist'
// import * as grpc from '@grpc/grpc-js'
// import * as protoLoader from '@grpc/proto-loader'

// var PROTO_PATH = process.cwd() + '/protos/helloworld.proto';

// // var parseArgs = require('minimist');
// // var grpc = require('@grpc/grpc-js');
// // var protoLoader = require('@grpc/proto-loader');
// var packageDefinition = protoLoader.loadSync(
//   PROTO_PATH,
//   {
//     keepCase: true,
//     longs: String,
//     enums: String,
//     defaults: true,
//     oneofs: true
//   });
// var hello_proto = grpc.loadPackageDefinition(packageDefinition).helloworld;
// var client = new hello_proto.Greeter('localhost:5152',
//   grpc.credentials.createInsecure());

// client.sayHello({ name: 'world' }, function (err, response) {
//   // console.log('Greeting:', response.message);
//   grpc = response.message;
// });

const postsDirectory = path.join(process.cwd(), 'posts')

export function getSortedPostsData() {
  // Get file names under /posts
  const fileNames = fs.readdirSync(postsDirectory)
  const allPostsData = fileNames.map(fileName => {
    // Remove ".md" from file name to get id
    const id = fileName.replace(/\.md$/, '')

    // Read markdown file as string
    const fullPath = path.join(postsDirectory, fileName)
    const fileContents = fs.readFileSync(fullPath, 'utf8')

    // Use gray-matter to parse the post metadata section
    const matterResult = matter(fileContents)

    // Combine the data with the id
    return {
      id,
      ...matterResult.data
    }
  })
  // Sort posts by date
  return allPostsData.sort((a, b) => {
    if (a.date < b.date) {
      return 1
    } else {
      return -1
    }
  })
}

export function getAllPostIds() {
  const fileNames = fs.readdirSync(postsDirectory);
  return fileNames.map((fileName) => {
    return {
      params: {
        id: fileName.replace(/\.md$/, ''),
      },
    };
  });
}

export function getPostData(id) {
  const fullPath = path.join(postsDirectory, `${id}.md`);
  const fileContents = fs.readFileSync(fullPath, 'utf8');

  // Use gray-matter to parse the post metadata section
  const matterResult = matter(fileContents);

  // Combine the data with the id
  return {
    id,
    ...matterResult.data,
  };
}

export async function getPostData1(id) {
  const fullPath = path.join(postsDirectory, `${id}.md`);
  const fileContents = fs.readFileSync(fullPath, 'utf8');

  // Use gray-matter to parse the post metadata section
  const matterResult = matter(fileContents);

  // Combine the data with the id
  return {
    id,
    ...matterResult.data,
  };
}



export function getgrpcdata(rpccallback) {

  // this.state
  // var [state, setstate]= useState('123')

  // var parseArgs = require('minimist');
  // var grpc = require('@grpc/grpc-js');
  // var protoLoader = require('@grpc/proto-loader');
  // var PROTO_PATH = process.cwd() + '/protos/helloworld.proto';
  // var packageDefinition = protoLoader.loadSync(
  //   PROTO_PATH,
  //   {
  //     keepCase: true,
  //     longs: String,
  //     enums: String,
  //     defaults: true,
  //     oneofs: true
  //   });
  // var hello_proto = grpc.loadPackageDefinition(packageDefinition).helloworld;
  // var client = new hello_proto.Greeter('localhost:5152',
  //   grpc.credentials.createInsecure());

  // client.sayHello({ name: 'world' }, function (err, response) {
  //   // console.log('Greeting:', response.message);
  //   // setallPostsData1(response.message);
  //   rpccallback(response.message)
  // });

  setTimeout(()=>{rpccallback('9999999999')})

  const allPostsData1 = JSON.stringify(getPostData1('ssg-ssr'))
  return '123456789';

  // var allPostsData1 = JSON.stringify(await getPostData1('ssg-ssr'));
  // const allPostsData = getSortedPostsData()
  // // const allPostsData1 = JSON.stringify(getPostData1('ssg-ssr'))
  // const getAllPostIdsvar = JSON.stringify(getAllPostIds())

}

export default async function Home() {
  const data = await fetch("http://www.baidu.com").then(res => res.text());
  return (
    <div dangerouslySetInnerHTML={{ __html: data }}></div>
  )
}