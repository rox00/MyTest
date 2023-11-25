import Layout from '../../components/layout';
import { getAllPostIds, getPostData } from '../../lib/posts.js';
import Head from 'next/head';

export default function Post({ postData }) {
  return (
    // <Layout>
    //   {postData.title}
    //   <br />
    //   {postData.id}
    //   <br />
    //   {postData.date}
    // </Layout>
    <>    
    <Head>
      <title>{postData.title}</title>
    </Head>
      {postData.title}
      <br />
      {postData.id}
      <br />
      {postData.date}
      <br />
      <Date dateString={postData.date}></Date>
    </>
  );
}

export async function getStaticPaths() {
  const paths = getAllPostIds();
  return {
    paths,
    fallback: false,
  };
}

export async function getStaticProps({ params }) {
  // const postData = getPostData(params.id);
  // return {
  //   props: {
  //     postData,
  //   },
  // };

  //for async
  const postData = getPostData(params.id)
    return {
    props: {
      postData,
    },
  };
}
