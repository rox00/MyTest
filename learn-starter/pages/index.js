import Head from 'next/head'
import Layout, { siteTitle } from '../components/layout'
import styles from '../styles/Home.module.css';
import utilStyles from '../styles/utils.module.css'
import { getSortedPostsData, getPostData1, getAllPostIds, getgrpcdata } from '../lib/posts.js';
import Date from '../components/date';
import Link from 'next/link';
import { useState } from 'react';
import axios from 'axios';


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

export default function Home({ allPostsData1, allPostsData, getAllPostIdsvar }) {
  var [grpcdata, setgrpcdata]= useState(0)

  var timer = setTimeout(async ()=>{
    let result = await fetch('http://localhost:9000').then(res => res.text());//.then(res => res.text())
    setgrpcdata(result)
  }, 3000)

  
  async function grpcclick(event){
    // let result =  await axios.get('http://myhost:3000/api/hello')
    // let result = await fetch('http://www.baidu.com').then(res => res.text());
    let result = await fetch('http://localhost:9000').then(res => res.text());//.then(res => res.text())
    // let result =  await fetch('http://localhost:3000/api/hello').then(res => res.text());

    result = JSON.stringify(result);
    // ,{
    //   headers:{'Content-Type': 'application/json',
    //   "Access-Control-Allow-Origin": "*",
    //   'Accept': 'application/json'},
    //     method: 'get',
    // }

  setgrpcdata(result);
    // var aa = document.getElementById('aaa')
    // const elementId = event.target.value;
    // alert(aa)
    // alert(typeof(aa))
    // aaa.text = '456789'
  //   allPostsData1 = getgrpcdata(a=>{
  //     allPostsData1= a;
  // });
    // return <text>FDSAFDSAFD</text>
    // clearTimeout(timer)
  }

  return (
    <Layout home>
      <Head>
        <title>{siteTitle}</title>
      </Head>
      <section className={utilStyles.headingMd}>
        <p>[Your Self Introduction]</p>
        <p>
          (This is a sample website - you’ll be building a site like this on{' '}
          <a href="https://www.nextjs.cn/learn">our Next.js tutorial</a>.)
        </p>
      </section>
      <button >{grpcdata}</button>
      <button onClick={grpcclick}>123456789</button>
      <section className={`${utilStyles.headingMd} ${utilStyles.padding1px}`}>
        <h2 className={utilStyles.headingLg}>Blog</h2>
        <ul className={utilStyles.list}>
          <text id='aaa'>getPostData1:: {allPostsData1}</text>
          {allPostsData.map(({ id, date, title }) => (
            <li className={utilStyles.listItem} key={id}>
              <Link href={`/posts/${id}`}>
                <>
                  <text>{title}</text>
                  <br />
                  <small className={utilStyles.lightText}>
                    <Date dateString={date} />
                  </small>
                </>
              </Link>
            </li>
            // <li className={utilStyles.listItem} key={id}>
            //   {title}
            //   <br />
            //   {id}
            //   <br />
            //   {date}
            // </li>
          ))}
          <text>getAllPostIds:: {getAllPostIdsvar}</text>
        </ul>
      </section>
    </Layout>
  )
}

export async function getStaticProps() {
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
  //   setallPostsData1(response.message);
  //   // allPostsData1 = response.message;
  // });


  // var allPostsData1= useState()
  // allPostsData1 = '123456'
  // var [allPostsData1, setallPostsData1]= useState('123')
  var allPostsData2 = JSON.stringify(await getPostData1('ssg-ssr'));
  var allPostsData1 = getgrpcdata(a=>{
      allPostsData1= a;
  });
  // var allPostsData1 = '123456789'
  const allPostsData = getSortedPostsData()
  // const allPostsData1 = JSON.stringify(getPostData1('ssg-ssr'))
  const getAllPostIdsvar = JSON.stringify(getAllPostIds())

  return {
    props: { allPostsData1, allPostsData, getAllPostIdsvar ,allPostsData2}
  }
}

